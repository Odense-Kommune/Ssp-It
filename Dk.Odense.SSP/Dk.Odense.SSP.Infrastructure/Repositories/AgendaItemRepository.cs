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
    public class AgendaItemRepository : BaseRepository<AgendaItem>, IAgendaItemRepository
    {
        public AgendaItemRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<IEnumerable<AgendaItem>> GetAgendaItemsWithWorriesFromAgendaId(Guid agendaId)
        {
            var res = await ListQuery().Where(q => q.Agenda_Id == agendaId).Include(a => a.Worries).AsNoTracking().ToListAsync();

            return res;
        }

        public async Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgenda(Guid agendaId)
        {
            var res = await DbSet.Where(q => q.Agenda_Id == agendaId)
                .Include(w => w.Worries)
                .ThenInclude(x => x.Reporter)
                
                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.Worries).AsTracking()

                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.Worries)
                .ThenInclude(x => x.AgendaItem).AsTracking()

                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.Worries)
                .ThenInclude(x => x.AgendaItem)
                .ThenInclude(x => x.Agenda).AsTracking()

                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.Worries)
                .ThenInclude(x => x.AgendaItem)
                .ThenInclude(x => x.Categorization).AsTracking()

                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(s => s.SchoolData)
                
                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.PersonGroupings)
                
                .Include(w => w.Worries)
                .ThenInclude(p => p.Person).ThenInclude(x => x.SspArea)

                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.PersonGroupings)
                .ThenInclude(x => x.Grouping)

                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.PersonGroupings)
                .ThenInclude(x=>x.Classification)

                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.SspArea)
                
                .Include(w => w.Worries)
                .ThenInclude(p => p.Person)
                .ThenInclude(x => x.SchoolData)
                
                .Include(x => x.Agenda)
                
                .Include(x => x.Categorization)
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgendaNoIncludes(Guid agendaId)
        {
            var res = await DbSet.Where(q => q.Agenda_Id == agendaId)
                .Include(x => x.Worries).ThenInclude(x => x.Person).AsNoTracking()
                .ToListAsync();

            return res;
        }

        public async Task<AgendaItem> GetAgendaItemWithIncludes(Guid id)
        {
            var res = await ListQuery().Where(x => x.Id == id)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Person)
                    .ThenInclude(x => x.Robustnesses)
                    .ThenInclude(x => x.Reporter)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Person)
                    .ThenInclude(x => x.Robustnesses)
                    .ThenInclude(x => x.Assessment)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Assessment)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Concern)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Person)
                    .ThenInclude(x => x.Notes)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Reporter)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Concern)
                    .Include(x => x.Agenda)

                    .Include(x => x.Worries)
                    .ThenInclude(x => x.Person).ThenInclude(x => x.Worries).AsTracking()
                    .FirstOrDefaultAsync();
            return res;
        }
    }
}
