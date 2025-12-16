using System.Threading.Tasks;
using Dk.Odense.SSP.ExternalServices.Model;

namespace Dk.Odense.SSP.ExternalServices.Interfaces
{
    public interface IServiceplatformIntegrationApiRepository
    {
        Task<SpPerson> GetSpPerson(string cpr);
    }
}
