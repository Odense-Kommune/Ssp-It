using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Application
{
    public class ReportedPersonService : BaseService<ReportedPerson, IReportedPersonRepository>, IReportedPersonService
    {
        public ReportedPersonService(IReportedPersonRepository repository) : base(repository)
        {
        }

        public async Task<ReportedPerson> GetBySocialSecNum(string socialSecNum)
        {
            return await Repository.GetBySocialSecNum(socialSecNum);
        }
    }
}
