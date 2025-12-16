using System;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class RobustnessRepository : BaseRepository<Robustness>, IRobustnessRepository
    {
        public RobustnessRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<int> GetCountByPersonId(Guid personId)
        {
            var res = await ListQuery().CountAsync(x => x.Person_Id == personId);

            return res;
        }

        public async Task<Robustness> GetNext(Guid personId, int increment = 0)
        {
            var res = await ListQuery().Where(x => x.Person_Id == personId && x.Increment > increment)
                .Include(x => x.ReportedPerson).AsNoTracking()
                .Include(x => x.Assessment).AsNoTracking()
                .Include(x => x.Reporter).AsNoTracking()
                .OrderBy(x => x.Increment).FirstOrDefaultAsync();

            return res;
        }

        public async Task<Robustness> GetPrevious(Guid personId, int increment = 0)
        {
            var res = await ListQuery().Where(x => x.Person_Id == personId && x.Increment < increment)
                .Include(x => x.ReportedPerson).AsNoTracking()
                .Include(x => x.Assessment).AsNoTracking()
                .Include(x => x.Reporter).AsNoTracking()
                .OrderByDescending(x => x.Increment).FirstOrDefaultAsync();

            return res;
        }
    }
}
