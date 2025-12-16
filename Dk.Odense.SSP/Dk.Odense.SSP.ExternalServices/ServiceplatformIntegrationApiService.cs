using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.ExternalServices.Interfaces;


namespace Dk.Odense.SSP.ExternalServices
{
    public class ServiceplatformIntegrationApiService : ICprService
    {
        private readonly IServiceplatformIntegrationApiRepository serviceplatformIntegrationApiRepository;

        public ServiceplatformIntegrationApiService(IServiceplatformIntegrationApiRepository serviceplatformIntegrationApiRepository)
        {
            this.serviceplatformIntegrationApiRepository = serviceplatformIntegrationApiRepository;
        }

        public async Task<NavneOpslagData> GetPerson(string cpr)
        {
            var res = await serviceplatformIntegrationApiRepository.GetSpPerson(cpr);

            return new NavneOpslagData()
            {
                Name = res.Name,
                Address = $"{res.Address}, {res.PostalCode} {res.PostalDistrict}",
                SocialSecNum = res.SocialSecurityNumber,
                Birthday= res.BirthDate
            };
        }
    }
}
