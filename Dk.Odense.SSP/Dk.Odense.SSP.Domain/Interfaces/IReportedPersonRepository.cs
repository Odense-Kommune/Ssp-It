using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IReportedPersonRepository : IBaseRepository<ReportedPerson>
    {
        Task<ReportedPerson> GetBySocialSecNum(string socialSecNum);
    }
}
