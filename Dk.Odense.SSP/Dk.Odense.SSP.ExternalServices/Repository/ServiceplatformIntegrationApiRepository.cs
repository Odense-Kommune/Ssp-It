using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.ExternalServices.Interfaces;
using Dk.Odense.SSP.ExternalServices.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Dk.Odense.SSP.ExternalServices.Repository
{
    public class ServiceplatformIntegrationApiRepository : IServiceplatformIntegrationApiRepository
    {
        private readonly ServiceplatformIntegrationApiConfig serviceplatformIntegrationApiConfig;
        private readonly HttpClient client;

        public ServiceplatformIntegrationApiRepository(IOptions<ServiceplatformIntegrationApiConfig> serviceplatformIntegrationApiConfig)
        {
            this.serviceplatformIntegrationApiConfig = serviceplatformIntegrationApiConfig.Value;
            this.client = CreateClient();
        }

        public async Task<SpPerson> GetSpPerson(string cpr)
        {
            string jsonBody = JsonConvert.SerializeObject(new
            {
                serviceplatformIntegrationApiConfig.ServiceAgreement,
                serviceplatformIntegrationApiConfig.SystemUser,
                serviceplatformIntegrationApiConfig.CertificateThumbprint,
                Value = cpr
            });

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(serviceplatformIntegrationApiConfig.Endpoint, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<SpPerson>(result);
            return data;
        }

        private HttpClient CreateClient()
        {
            HttpClient httpClient = new()
            {
                BaseAddress = new Uri(serviceplatformIntegrationApiConfig.Endpoint)
            };

            return httpClient;
        }
    }
}
