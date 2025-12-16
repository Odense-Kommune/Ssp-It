
using Dk.Odense.SSP.SagOgDokIndeks.Config;
using Microsoft.Extensions.Configuration;

namespace Dk.Odense.SSP.SagOgDokIndeks.Helpers
{
    internal class OrganisationRegistryProperties
    {
        [ThreadStatic]
        private static string MunicipalityThreadValue;

        public static AppSettings AppSettings { get; set; } = new AppSettings();

        public static Dictionary<string, string> MunicipalityOrganisationUUID { get; set; }

        // prevent instances
        private OrganisationRegistryProperties()
        {
        }

        static OrganisationRegistryProperties()
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static string GetCurrentMunicipality()
        {
            if (string.IsNullOrEmpty(MunicipalityThreadValue))
            {
                return AppSettings.SODIConfig.Cvr;
            }

            return MunicipalityThreadValue;
        }

        public static void SetCurrentMunicipality(string municipality)
        {
            if (!string.IsNullOrEmpty(municipality))
            {
                MunicipalityThreadValue = municipality;
            }
            else
            {
                MunicipalityThreadValue = AppSettings.SODIConfig.Cvr;
            }
        }

        private static void Init()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .AddEnvironmentVariables()
                 .Build();

            configuration.Bind(AppSettings);

            // InitLog();

            switch (AppSettings.SODIConfig.Environment)
            {
                case "TEST":
                    AppSettings.SODIConfig.StsEndpointAddress = "https://n2adgangsstyring.eksterntest-stoettesystemerne.dk/runtime/services/kombittrust/14/certificatemixed";
                    AppSettings.SODIConfig.StsEntityIdentifier = "https://saml.adgangsstyring.eksterntest-stoettesystemerne.dk/runtime";
                    break;
                case "PROD":
                    AppSettings.SODIConfig.StsEndpointAddress = "https://n2adgangsstyring.stoettesystemerne.dk/runtime/services/kombittrust/14/certificatemixed";
                    AppSettings.SODIConfig.StsEntityIdentifier = "https://saml.adgangsstyring.stoettesystemerne.dk/runtime";
                    break;
                default:

                    break;
            }

            // list of all 98 municipalities
            MunicipalityOrganisationUUID = new Dictionary<string, string>();
            MunicipalityOrganisationUUID["35209115"] = "76b8ef41-7e58-4d29-b2e9-f8c72f89eb7d";

            // load custom if available
            if (!string.IsNullOrEmpty(AppSettings.SODIConfig.CvrUuid))
            {
                MunicipalityOrganisationUUID[AppSettings.SODIConfig.Cvr] = AppSettings.SODIConfig.CvrUuid;
            }
        }
    }
}