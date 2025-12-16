using System.Threading.Tasks;
using Dk.Odense.SSP.Sbsys.Model;

namespace Dk.Odense.SSP.Sbsys.Interfaces
{
    public interface ISbsysCaseRepository
    {
        Task<SbsysCaseList> GetCases(string socialSecNum);
    }
}
