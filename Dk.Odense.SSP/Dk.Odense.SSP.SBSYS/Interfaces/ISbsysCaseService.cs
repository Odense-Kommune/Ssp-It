using System.Threading.Tasks;
using Dk.Odense.SSP.Sbsys.Model;

namespace Dk.Odense.SSP.Sbsys.Interfaces
{
    public interface ISbsysCaseService
    {
        Task<SbsysCaseList> GetCases(string socialSecNum);
    }
}
