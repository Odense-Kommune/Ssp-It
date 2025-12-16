using System.Threading.Tasks;
using Dk.Odense.SSP.ExternalServices.Model;

namespace Dk.Odense.SSP.ExternalServices.Interfaces
{
    public interface ISchoolAreaService
    {
        Task<SchoolDataReturn> GetSchoolData(string socialSecNum, string username, int retries = 0);
    }
}
