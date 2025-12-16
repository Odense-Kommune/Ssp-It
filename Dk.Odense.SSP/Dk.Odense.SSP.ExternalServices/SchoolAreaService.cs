using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.ExternalServices.Interfaces;
using Dk.Odense.SSP.ExternalServices.Model;
using Microsoft.Extensions.Options;
using PladshistorikWs;
using UvvejWs;

namespace Dk.Odense.SSP.ExternalServices
{
    public class SchoolAreaService : ISchoolAreaService
    {
        private readonly pladshistorikWSSoapClient clientPladshistorikWsSoapClient;
        private readonly serviceSoapClient clientUvvejSoapClient;
        private readonly ExternalConfig config;

        public SchoolAreaService(IOptions<ExternalConfig> config)
        {
            this.config = config.Value;

            clientPladshistorikWsSoapClient = new pladshistorikWSSoapClient(
                pladshistorikWSSoapClient.EndpointConfiguration.pladshistorikWSSoap12,
                this.config.PladshistorikEndpoint
            );
            clientUvvejSoapClient = new serviceSoapClient(
                serviceSoapClient.EndpointConfiguration.serviceSoap12,
                this.config.UvvejEndpoint
            );
        }

        public async Task<SchoolDataReturn> GetSchoolData(
            string socialSecNum,
            string username,
            int retries = 0
        )
        {
            try
            {
                //Pladshistorik active
                if (socialSecNum == null)
                    return new SchoolDataReturn();
                var pladshistorikList = await GetPladshistorikData(socialSecNum, username);
                var schoolDataPladsList = pladshistorikList
                    .Select(x => new SchoolData()
                    {
                        Name = x.AfdelingsNavn ?? "",
                        DateFirst = DateTime.Parse(x.dateFirst, new CultureInfo("da-DK", false)),
                        DateLast = DateTime.Parse(x.dateLast, new CultureInfo("da-DK", false)),
                    })
                    .OrderByDescending(x => x.DateLast)
                    .ToList();

                foreach (var schoolData in schoolDataPladsList)
                {
                    if (
                        schoolData.DateLast > DateTime.Now
                        && schoolData.DateFirst < DateTime.Now
                        && schoolData.Name.Contains("SFO")
                    )
                        return SchoolDataReturnMap(schoolData);
                }

                // Uvvej active
                var uvvejList = await GetUvvejData(socialSecNum, username);

                var schoolDataUvvejList = uvvejList
                    .Select(x => new SchoolData()
                    {
                        Name =
                            string.IsNullOrEmpty(x.Sted) || x.Sted == "Skat"
                                ? x.Underplacering
                                : x.Sted,
                        DateFirst = DateTime.Parse(x.StartDato, new CultureInfo("da-DK", false)),
                        DateLast =
                            x.SlutDato != ""
                                ? DateTime.Parse(x.SlutDato, new CultureInfo("da-DK", false))
                            : x.Status == "Afsluttet"
                                ? DateTime.Parse(x.StartDato, new CultureInfo("da-DK", false))
                            : DateTime.MaxValue,
                        Type = x.Status ?? "",
                    })
                    .OrderByDescending(x => x.DateLast)
                    .ToList();

                foreach (var schoolData in schoolDataUvvejList)
                {
                    if (
                        schoolData.DateLast > DateTime.Now
                        && schoolData.DateFirst < DateTime.Now
                        && schoolData.Type != "Tilmeldt"
                    )
                        return SchoolDataReturnMap(schoolData);
                }

                // Find Inactive
                var schoolDataAll = new List<SchoolData>();
                schoolDataAll.AddRange(schoolDataPladsList);
                schoolDataAll.AddRange(schoolDataUvvejList);
                schoolDataAll = schoolDataAll.OrderByDescending(x => x.DateLast).ToList();

                foreach (var schoolData in schoolDataAll)
                {
                    if (
                        schoolData.DateFirst < DateTime.Now
                        && (schoolData.Type != "Tilmeldt" || schoolData.Name.Contains("SFO"))
                    )
                    {
                        return SchoolDataReturnMap(schoolData);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message != "An error occurred while sending the request.")
                {
                    return new SchoolDataReturn()
                    {
                        Name = "Ingen skole fundet, FEJL!",
                        DateFirst = DateTime.MinValue,
                        DateLast = DateTime.MaxValue,
                    };
                }

                if (retries != 10)
                {
                    Thread.Sleep(15000);

                    return await GetSchoolData(socialSecNum, username, retries + 1);
                }
                Console.WriteLine(e);
                return new SchoolDataReturn()
                {
                    Name = "Ingen skole fundet, FEJL!",
                    DateFirst = DateTime.MinValue,
                    DateLast = DateTime.MaxValue,
                };
            }
            return new SchoolDataReturn()
            {
                Name = "Ingen skole fundet",
                DateFirst = DateTime.MinValue,
                DateLast = DateTime.MaxValue,
            };
        }

        private async Task<tilbudshistorik[]> GetPladshistorikData(
            string socialSecNum,
            string username
        )
        {
            return await clientPladshistorikWsSoapClient.GetPladshistorikAlleAsync(
                config.SystemUser,
                config.SystemPassword,
                username,
                socialSecNum
            );
        }

        private async Task<Forloeb[]> GetUvvejData(string socialSecNum, string username)
        {
            return await clientUvvejSoapClient.getForloebAsync(
                config.SystemUser,
                config.SystemPassword,
                "ssp",
                socialSecNum
            );
        }

        private static SchoolDataReturn SchoolDataReturnMap(SchoolData schoolData)
        {
            return new SchoolDataReturn()
            {
                Name = schoolData.Name,
                DateFirst = schoolData.DateFirst,
                DateLast = schoolData.DateLast,
            };
        }
    }
}
