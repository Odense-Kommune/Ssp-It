using System.Threading.Tasks;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.ExternalServices.Interfaces
{
    public interface ICprService
    {
        Task<NavneOpslagData> GetPerson(string cpr);
    }
}
