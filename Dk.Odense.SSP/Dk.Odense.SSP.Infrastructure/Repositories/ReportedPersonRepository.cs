using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class ReportedPersonRepository : BaseRepository<ReportedPerson> , IReportedPersonRepository
    {
        public ReportedPersonRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<ReportedPerson> GetBySocialSecNum(string socialSecNum)
        {
            return await ListQuery().FirstOrDefaultAsync(x => x.ReportedCpr == socialSecNum);
        }
    }
}
