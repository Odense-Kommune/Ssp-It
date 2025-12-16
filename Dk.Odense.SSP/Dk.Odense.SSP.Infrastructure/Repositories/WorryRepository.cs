using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class WorryRepository : BaseRepository<Worry>, IWorryRepository
    {
        public WorryRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {

        }

        public async Task<int> GetCountByPersonId(Guid personId)
        {
            return await ListQuery().CountAsync(x => x.Person_Id == personId);
        }

        public async Task<Worry> GetWithAgendaItems(Guid id)
        {
            var res = await ListQuery().Where(x => x.Id == id)
                .Include(x => x.AgendaItem).ThenInclude(x => x.Worries).AsNoTracking()
                .ToListAsync();

            return res.FirstOrDefault();
        }

        public async Task<Worry> GetNext(Guid personId, int increment)
        {
            var res = await ListQuery().Where(x => x.Person_Id == personId && x.Increment > increment)
                .Include(x => x.Concern).AsNoTracking()
                .Include(x => x.Assessment).AsNoTracking()
                .Include(x => x.Reporter).AsNoTracking()
                .OrderBy(x => x.Increment).FirstOrDefaultAsync();

            return res;
        }

        public async Task<Worry> GetPrevious(Guid personId, int increment)
        {
            var res = await ListQuery().Where(x => x.Person_Id == personId && x.Increment < increment)
                .Include(x => x.Concern).AsNoTracking()
                .Include(x => x.Assessment).AsNoTracking()
                .Include(x => x.Reporter).AsNoTracking()
                .OrderByDescending(x => x.Increment).FirstOrDefaultAsync();

            return res;
        }

        public async Task<IEnumerable<Worry>> GetToBeVerified()
        {
            var res = await ListQuery().Where(x => x.PendingAutoVerify)
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<Worry>> GetToBeVerifiedWithIncludes()
        {
            var res = await ListQuery().Where(x => x.PendingAutoVerify)
                .Include(q => q.ReportedPerson).AsNoTracking()
                .ToListAsync();

            return res;
        }


        public async Task<IEnumerable<Worry>> GetFromPersonId(Guid personId)
        {
            var res = await ListQuery().Where(x => x.Person_Id == personId)
                .Include(x => x.Reporter).AsNoTracking()
                .Include(x => x.AgendaItem).ThenInclude(x => x.Agenda).AsNoTracking()
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<Worry>> GetNotPendingWithIncludes()
        {
            var res = await ListQuery().Where(x => x.AgendaItem_Id == null && !x.PendingAutoVerify)
                .Include(x => x.ReportedPerson).AsNoTracking()
                .Include(x => x.Reporter).AsNoTracking()
                .Include(x => x.Source).AsNoTracking()
                .Include(x => x.Person).AsNoTracking()
                .ToListAsync();

            return res;
        }

        public async Task<Worry> GetWithIncludes(Guid id)
        {
            var res = await ListQuery().Where(x => x.Id == id)
                .Include(x => x.Person)
                .Include(x => x.PoliceWorryCategory)
                .Include(x => x.PoliceWorryRole)
                .Include(x => x.Assessment)
                .Include(x => x.Concern)
                .Include(x => x.Reporter)
                .FirstOrDefaultAsync();

            return res;
        }

        public async Task<IEnumerable<Worry>> GetGroundless()
        {
            var res = await ListQuery().Where(x => x.Groundless != null && x.Groundless < DateTime.Now.Date.AddDays(-7))
                .AsNoTracking()
                .ToListAsync();

            return res;
        }
    }
}
