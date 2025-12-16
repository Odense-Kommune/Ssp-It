using System.Globalization;
using Dk.Odense.SSP.Application;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.ExternalServices;
using Dk.Odense.SSP.ExternalServices.Interfaces;
using Dk.Odense.SSP.ExternalServices.Repository;
using Dk.Odense.SSP.Gdf;
using Dk.Odense.SSP.Gdf.Model;
using Dk.Odense.SSP.Gdf.Repository;
using Dk.Odense.SSP.Gdf.Repository.Interface;
using Dk.Odense.SSP.Gdf.Services;
using Dk.Odense.SSP.Gdf.Services.Interface;
using Dk.Odense.SSP.Infrastructure;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Dk.Odense.SSP.Infrastructure.Repositories;
using Dk.Odense.SSP.Infrastructure.UnitOfWork;
using Dk.Odense.SSP.Logger;
using Dk.Odense.SSP.SagOgDokIndeks.Repositories;
using Dk.Odense.SSP.SagOgDokIndeks.Services;
using Dk.Odense.SSP.Sbsys;
using Dk.Odense.SSP.Sbsys.Interfaces;
using Dk.Odense.SSP.Sbsys.Repository;
using Dk.Odense.SSP.UserCase;
using Dk.Odense.SSP.Web.Auth;
using Dk.Odense.SSP.Web.Controllers.Api;
using Dk.Odense.SSP.Web.Helpers;
using Dk.Odense.SSP.Xflow.Interfaces;
using Dk.Odense.SSP.Xflow.Repositories;
using Dk.Odense.SSP.Xflow.Services;
using Dk.Odense.SSP.Xflow.UseCase;
using Hangfire;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.OpenApi.Models;

namespace Dk.Odense.SSP.Web
{
    public class Startup
    {
        private IServiceCollection serviceCollection;
        private readonly IWebHostEnvironment hostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            hostingEnvironment = env;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InjectDbServices(services);
            InjectApplication(services);
            InjectInfrastructure(services);
            InjectUseCases(services);
            InjectBaseAndLogging(services);
            InjectExternalAndTools(services);
            SbsysInjection(services);
            SwaggerServiceConfig(services);
            ReadConfig(services);

            if (hostingEnvironment.IsDevelopment())
            {
                AzureServiceConfiguration(services);

                //services.AddAuthorization(opt =>
                //    opt.AddPolicy("SSP", builder => builder.RequireAssertion(_ => true).Build()));
            }
            else
            {
                AzureServiceConfiguration(services);
            }

            services.AddScoped<DeleteLogicController>();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("da-DK"),
                    new CultureInfo("en-DK"),
                };

                opts.DefaultRequestCulture = new RequestCulture("da-DK");
                // Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;
            });

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
                });

            services.AddSpaStaticFiles(conf =>
            {
                conf.RootPath = "wwwroot";
            });

            //Service Config
            services.AddCors();
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHangfireServer(options => options.WorkerCount = 1);

            services.AddMvc(opt => opt.EnableEndpointRouting = false);

            serviceCollection = services;
            services.AddHttpContextAccessor();

            services.AddApplicationInsightsTelemetry(o =>
            {
                o.EnableDependencyTrackingTelemetryModule = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            }
            else
            {
                app.UseHsts();
            }

            SetUiCulture(app);

            SwaggerAppConfig(app);

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            HangfireConfigure(app, env);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvcWithDefaultRoute();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
            });

            serviceCollection.BuildServiceProvider().GetService<HangfireManager>().RecurringJobs();

            app.UseDefaultFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "/";
            });
        }

        #region ConfigService

        private void ReadConfig(IServiceCollection services)
        {
            services.Configure<ExternalConfig>(Configuration.GetSection("ExternalConfig"));
            services.Configure<SbsysConfig>(Configuration.GetSection("SBSYSConfig"));
            services.Configure<ServiceplatformIntegrationApiConfig>(
                Configuration.GetSection("ServiceplatformIntegrationApi")
            );
            services.Configure<MailConfig>(Configuration.GetSection("MailConfig"));
            services.Configure<HangfireJobConfig>(Configuration.GetSection("HangfireJobConfig"));
            services.Configure<DevelopmentSettingsConfig>(
                Configuration.GetSection("DevelopmentSettingsConfig")
            );
            services.Configure<XFlowConfig>(Configuration.GetSection("XFlowConfig"));
        }
        #endregion

        #region AzureSetup
        private void AzureServiceConfiguration(IServiceCollection services)
        {
            services
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("MSAL:Auth"));

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("SSP", policy => policy.RequireRole(Configuration["Roles:SspRole"]));
            });

            services.AddRazorPages().AddMicrosoftIdentityUI();
        }
        #endregion

        #region dbServices
        private void InjectDbServices(IServiceCollection services)
        {
            services.AddDbContext<SspDbContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            ); //DefaultConnection

            services.AddDbContextPool<LogDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            ); //DefaultConnection

            //GDF Injection
            services.AddDbContext<GdfDbContext<AvaDev>>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GdfConnection"))
            );

            services.AddDbContext<GdfDbContext<AvaProd>>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GdfConnection"))
            );

            services.AddDbContext<GdfDbContext<RobusthedDev>>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GdfConnection"))
            );
            services.AddDbContext<GdfDbContext<RobusthedProd>>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GdfConnection"))
            );

            services.AddScoped<GdfDbContext<AvaDev>>();
            services.AddScoped<GdfDbContext<AvaProd>>();
            services.AddScoped<GdfDbContext<RobusthedDev>>();
            services.AddScoped<GdfDbContext<RobusthedProd>>();

            services.AddScoped<IGdfService<AvaDev>, AvaService<AvaDev>>();
            services.AddScoped<IGdfRepository<AvaDev>, AvaRepository<AvaDev>>();

            services.AddScoped<IGdfService<AvaProd>, AvaService<AvaProd>>();
            services.AddScoped<IGdfRepository<AvaProd>, AvaRepository<AvaProd>>();

            services.AddScoped<IGdfService<RobusthedDev>, RobustService<RobusthedDev>>();
            services.AddScoped<IGdfRepository<RobusthedDev>, RobusthedRepository<RobusthedDev>>();

            services.AddScoped<IGdfService<RobusthedProd>, RobustService<RobusthedProd>>();
            services.AddScoped<IGdfRepository<RobusthedProd>, RobusthedRepository<RobusthedProd>>();
        }
        #endregion

        #region SBSYS Injection

        private static void SbsysInjection(IServiceCollection services)
        {
            services.AddScoped<ISbsysCaseService, SbsysCaseService>();
            services.AddScoped<ISbsysCaseRepository, SbsysCaseRepository>();
        }

        #endregion

        #region applicationServices
        private void InjectApplication(IServiceCollection services)
        {
            services.AddScoped<IAgendaService, AgendaService>();
            services.AddScoped<IAgendaItemService, AgendaItemService>();
            services.AddScoped<IAreaRuleService, AreaRuleService>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddScoped<ICategorizationService, CategorizationService>();
            services.AddScoped<IClassificationService, ClassificationService>();
            services.AddScoped<IConcernService, ConcernService>();
            services.AddScoped<IGroupingService, GroupingService>();
            services.AddScoped<INoteSharedService, NoteSharedService>();
            services.AddScoped<INoteAdditionalService, NoteAdditionalService>();
            services.AddScoped<IPersonGroupingService, PersonGroupingService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPoliceWorryCategoryService, PoliceWorryCategoryService>();
            services.AddScoped<IPoliceWorryRoleService, PoliceWorryRoleService>();
            services.AddScoped<IReportedPersonService, ReportedPersonService>();
            services.AddScoped<IReporterService, ReporterService>();
            services.AddScoped<IRobustnessService, RobustnessService>();
            services.AddScoped<ISchoolDataService, SchoolDataService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<ISspAreaService, SspAreaService>();
            services.AddScoped<IWorryService, WorryService>();
        }
        #endregion

        #region infrastructureServices

        private static void InjectInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IAgendaRepository, AgendaRepository>();
            services.AddScoped<IAgendaItemRepository, AgendaItemRepository>();
            services.AddScoped<IAreaRuleRepository, AreaRuleRepository>();
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<ICategorizationRepository, CategorizationRepository>();
            services.AddScoped<IClassificationRepository, ClassificationRepository>();
            services.AddScoped<IConcernRepository, ConcernRepository>();
            services.AddScoped<IGroupingRepository, GroupingRepository>();
            services.AddScoped<INoteSharedRepository, NoteSharedRepository>();
            services.AddScoped<INotesAdditionalRepository, NoteAdditionalRepository>();
            services.AddScoped<IPersonGroupingRepository, PersonGroupingRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPoliceWorryCategoryRepository, PoliceWorryCategoryRepository>();
            services.AddScoped<IPoliceWorryRoleRepository, PoliceWorryRoleRepository>();
            services.AddScoped<IReportedPersonRepository, ReportedPersonRepository>();
            services.AddScoped<IReporterRepository, ReporterRepository>();
            services.AddScoped<IRobustnessRepository, RobustnessRepository>();
            services.AddScoped<ISchoolDataRepository, SchoolDataRepository>();
            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddScoped<ISspAreaRepository, SspAreaRepository>();
            services.AddScoped<IWorryRepository, WorryRepository>();
            services.AddScoped<IXFlowRobustnessRepository, XFlowRobustnessRepository>();
            services.AddScoped<IXFlowWorryRepository, XFlowWorryRepository>();
        }
        #endregion

        #region useCaseServices

        private static void InjectUseCases(IServiceCollection services)
        {
            services.AddScoped<AgendaOverviewUseCase>();
            services.AddScoped<AgendaSpecificUseCase>();
            services.AddScoped<ClassificationUseCase>();
            services.AddScoped<UploadUseCase>();
            services.AddScoped<ExportUseCase>();
            services.AddScoped<GroupingUseCase>();
            services.AddScoped<PersonUseCase>();
            services.AddScoped<ReportedPersonUseCase>();
            services.AddScoped<SchoolDataUseCase>();
            services.AddScoped<SourceUseCase>();
            services.AddScoped<SpecificPersonUseCase>();
            services.AddScoped<VerifyWorryUseCase>();
            services.AddScoped<ExportAgendaItemUseCase>();
            services.AddScoped<DeleteLogicUseCase>();
            services.AddScoped<XFlowRobustnessUseCase>();
            services.AddScoped<XFlowWorryUseCase>();
        }
        #endregion

        #region injectBaseServices

        private static void InjectBaseAndLogging(IServiceCollection services)
        {
            // db things
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDatabaseContext, SspDbContext>();

            // base services
            services.AddScoped<IBaseService<Categorization>, CategorizationService>();
            services.AddScoped<IBaseService<PoliceWorryCategory>, PoliceWorryCategoryService>();
            services.AddScoped<IBaseService<PoliceWorryRole>, PoliceWorryRoleService>();
            services.AddScoped<IBaseService<SspArea>, SspAreaService>();

            //CalcWeekNumber
            services.AddScoped<CalcWeekNumber>();

            // logger
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<ILoggerRepository, LoggerRepository>();
            services.AddSingleton<LogDbContext>();
        }
        #endregion

        #region InjectExternalsAndTools
        private static void InjectExternalAndTools(IServiceCollection services)
        {
            services.AddScoped<ISchoolAreaService, SchoolAreaService>();
            //services.AddScoped<ICprService, TempCprService>();
            services.AddScoped<ICprService, ServiceplatformIntegrationApiService>();
            services.AddScoped<
                IServiceplatformIntegrationApiRepository,
                ServiceplatformIntegrationApiRepository
            >();
            services.AddScoped<IXFlowRobustnessService, XFlowRobustnessService>();
            services.AddScoped<IXFlowWorryService, XFlowWorryService>();
            services.AddScoped<ISODIService, SODIService>();
            services.AddScoped<ISODIRepository, SODIRepository>();
            services.AddScoped<HangFireUseCase>();
            services.AddScoped<HangfireManager>();
        }

        private static void SetUiCulture(IApplicationBuilder app)
        {
            var defaultInfo = new RequestCulture("da-DK");

            var supportedCultures = new[] { new CultureInfo("da-DK"), new CultureInfo("en-DK") };

            app.UseRequestLocalization(
                new RequestLocalizationOptions
                {
                    DefaultRequestCulture = defaultInfo,
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures,
                }
            );
        }
        #endregion

        #region HangfireSetup
        private static void HangfireConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions()
                {
                    Authorization = new[] { new HangfireAuthorizationFilter(env) },
                }
            );
        }
        #endregion

        #region Swagger
        private static void SwaggerServiceConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        private static void SwaggerAppConfig(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSP API");
            });
        }
        #endregion
    }
}
