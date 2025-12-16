using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IReportedPersonService : IBaseService<ReportedPerson>
    {
        Task<ReportedPerson> GetBySocialSecNum(string socialSecNum);
    }
}
