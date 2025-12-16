using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Sbsys.Interfaces;
using Dk.Odense.SSP.Sbsys.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Dk.Odense.SSP.Sbsys.Repository
{
    public class SbsysCaseRepository : ISbsysCaseRepository
    {
        private readonly SbsysConfig sbsysConfig;
        private readonly HttpClient client;

        public SbsysCaseRepository(IOptions<SbsysConfig> sbsysConfig)
        {
            this.sbsysConfig = sbsysConfig.Value;
            this.client = CreateClient();
        }

        public async Task<SbsysCaseList> GetCases(string socialSecNum)
        {
            var token = await GetAccessToken();

            var result = await GetCasesSbsip(socialSecNum, token);

            return result;
        }

        private async Task<SbsysCaseList> GetCasesSbsip(string socialSecNum, string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            var searchRequest = new SearchRequest
            {
                AllePersoner = new PersonData { CprNummer = socialSecNum },
            };
            var searchJson = JsonSerializer.Serialize(searchRequest);
            var stringContent = new StringContent(searchJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(
                sbsysConfig.SbsipEndpoint + "/sag/search",
                stringContent
            );

            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            JArray jsonSager = (JArray)jsonObject["Results"]!;
            var cases = new SbsysCaseList { Sager = new List<SbsysCase>() };
            foreach (var jsonSag in jsonSager)
            {
                cases.Sager.Add(
                    new SbsysCase()
                    {
                        Id = new Guid(jsonSag["SagIdentity"]!.ToString()),
                        Sagsstatus = jsonSag["SagsStatus"]!["Navn"]!.ToString(),
                        Sagstilstand = jsonSag["SagsStatus"]!["SagsTilstand"]!.ToString(),
                        SagSkabelonId = jsonSag["SagSkabelon"]?["SagSkabelonIdentity"]?.ToString(),
                    }
                );
            }
            return cases;
        }

        private async Task<string> GetAccessToken()
        {
            try
            {
                var httpClient = new HttpClient();
                var formdata = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "client_id", sbsysConfig.TokenClientId },
                    { "client_secret", sbsysConfig.TokenClientSecret },
                    { "username", sbsysConfig.TokenUser },
                    { "password", sbsysConfig.TokenPass },
                };
                var requestBody = new FormUrlEncodedContent(formdata);

                var response = await httpClient.PostAsync(
                    sbsysConfig.SbsipTokenEndpoint,
                    requestBody
                );
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var token = JsonSerializer.Deserialize<AccessTokenModel>(result);

                return token!.AccessToken;
            }
            catch (WebException ex)
            {
                await using var stream = ex.Response!.GetResponseStream();
                if (stream == null)
                    throw;

                using var reader = new StreamReader(stream);

                var error = await reader.ReadToEndAsync();

                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public class SearchRequest
        {
            public PersonData AllePersoner { get; set; }
        }

        public class PersonData
        {
            public string CprNummer { get; set; }
        }

        private HttpClient CreateClient()
        {
            HttpClient httpClient = new();

            httpClient.BaseAddress = new Uri(sbsysConfig.SbsipTokenEndpoint);

            return httpClient;
        }
    }
}
