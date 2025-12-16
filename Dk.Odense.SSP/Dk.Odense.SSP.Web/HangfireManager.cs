using Dk.Odense.SSP.UserCase;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq.Expressions;
using Dk.Odense.SSP.Gdf.Model;
using Dk.Odense.SSP.Gdf.Services.Interface;
using Dk.Odense.SSP.Web.Controllers.Api;
using Microsoft.Extensions.Hosting;
using Dk.Odense.SSP.Xflow.UseCase;

namespace Dk.Odense.SSP.Web
{
    public class HangfireManager
    {
        private readonly IGdfService<AvaDev> avaDev;
        private readonly IGdfService<AvaProd> avaProd;
        private readonly IGdfService<RobusthedProd> robusthedProd;
        private readonly IGdfService<RobusthedDev> robusthedDev;
        private readonly HangFireUseCase hangFireUseCase;
        private readonly XFlowWorryUseCase xFlowWorryUseCase;
        private readonly XFlowRobustnessUseCase xFlowRobustnessUseCase;
        private readonly IWebHostEnvironment env;
        private readonly DeleteLogicController deleteLogicController;

        public HangfireManager(
            IGdfService<AvaDev> avaDev, 
            IGdfService<AvaProd> avaProd, 
            IGdfService<RobusthedProd> robusthedProd, 
            IGdfService<RobusthedDev> robusthedDev,
            XFlowWorryUseCase xFlowWorryUseCase,
            XFlowRobustnessUseCase xFlowRobustnessUseCase,
            HangFireUseCase hangFireUseCase, 
            IWebHostEnvironment env, 
            DeleteLogicController deleteLogicController)
        {
            this.avaDev = avaDev;
            this.avaProd = avaProd;
            this.robusthedDev = robusthedDev;
            this.robusthedProd = robusthedProd;
            this.env = env;
            this.deleteLogicController = deleteLogicController;
            this.xFlowWorryUseCase = xFlowWorryUseCase;
            this.xFlowRobustnessUseCase = xFlowRobustnessUseCase;
            this.hangFireUseCase = hangFireUseCase;
        }

        public HangfireManager(DeleteLogicController deleteLogicController)
        {
            this.deleteLogicController = deleteLogicController;
        }

        public void RecurringJobs()
        {
            if (!env.IsProduction())
            {
                RecurringJob.AddOrUpdate("UpdateSchoolData-Dev", () => hangFireUseCase.UpdateAllSchoolData(), Cron.Yearly(1, 1));
                //RecurringJob.AddOrUpdate("GetAva-Dev", () => avaDev.LoadData(), Cron.Yearly(1, 1)); // OLD JOB
                RecurringJob.AddOrUpdate("GetXFlowWorries-Dev", () => hangFireUseCase.RunXFlowWorryFlow(), Cron.Yearly(1, 1));
                //RecurringJob.AddOrUpdate("GetRobust-Dev", () => robusthedDev.LoadData(), Cron.Yearly(1, 1)); //OLD JOB
                RecurringJob.AddOrUpdate("GetXFlowRobustnesses-Dev", () => hangFireUseCase.RunXFlowRobustnessesFlow(), Cron.Yearly(1,1));
                RecurringJob.AddOrUpdate("Delete-Data-Dev", () => hangFireUseCase.DeleteExpiredData(), Cron.Yearly(1, 1));
                RecurringJob.AddOrUpdate("Set SspArea", () => hangFireUseCase.UpdateSspArea(), Cron.Yearly(1, 1));
            }
            else if (env.IsProduction())
            {
                //RecurringJob.AddOrUpdate("GetAva", () => avaProd.LoadData(), Cron.Hourly);
                RecurringJob.AddOrUpdate("GetXFlowWorries", () => hangFireUseCase.RunXFlowWorryFlow(), Cron.Hourly); //NEW JOB
                //RecurringJob.AddOrUpdate("GetRobust", () => robusthedProd.LoadData(), Cron.Hourly);
                RecurringJob.AddOrUpdate("GetXFlowRobustnesses", () => hangFireUseCase.RunXFlowRobustnessesFlow(), Cron.Hourly); //NEW JOB
                RecurringJob.AddOrUpdate("Delete-Data", () => hangFireUseCase.DeleteExpiredData(), Cron.Monthly(1));
                RecurringJob.AddOrUpdate("SendDeleteMailAdvis", () => deleteLogicController.SendMail(), Cron.Monthly(14));
                RecurringJob.AddOrUpdate("UpdateSchoolData", () => hangFireUseCase.UpdateAllSchoolData(), Cron.Daily(3), new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc
                });
                RecurringJob.AddOrUpdate("Delete Groundless", () => hangFireUseCase.DeleteGroundless(), Cron.Weekly(DayOfWeek.Sunday), new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc
                });
                RecurringJob.AddOrUpdate("Set SspArea", () => hangFireUseCase.UpdateSspArea(), Cron.Daily(1), new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc
                });
                
            }
        }

        public void EnqueueVerify()
        {
            BackgroundJob.Enqueue(() => hangFireUseCase.VerifyWorries());
        }

        public void EnqueueJob(Expression<Action> methodCall)
        {
            BackgroundJob.Enqueue(methodCall);
        }
    }
}
