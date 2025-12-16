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
    public class AgendaRepository : BaseRepository<Agenda>, IAgendaRepository
    {
        public AgendaRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<IEnumerable<Person>> GetAllPersonsOnAgenda(Guid agendaId)
        {
            var res = await ListQuery().Include(x => x.AgendaItems).ThenInclude(x => x.Worries)
                .ThenInclude(x => x.Person).AsNoTracking().Where(x => x.Id == agendaId)
                .SelectMany(x => x.AgendaItems.SelectMany(z => z.Worries.Select(y => y.Person))).Distinct().ToListAsync();

            return res;
        }

        public async Task<IEnumerable<Agenda>> GetHeldAgendas()
        {
            var res = await ListQuery().Where(x => x.MeetingHeld == true).OrderByDescending(x => x.Date).ThenBy(x => x.AgendaNumber).ToListAsync();

            return res;
        }

        public async Task<IEnumerable<Agenda>> GetCurrentAgendasWithAgendaItems()
        {
            var res = await ListQuery()
                      .Include(x => x.AgendaItems)
                      .ThenInclude(x => x.Worries).Where(x => x.MeetingHeld == false)
                      .Select(x => new Agenda()
                      {
                          Id = x.Id,
                          AgendaName = x.AgendaName,
                          AgendaSent = x.AgendaSent,
                          Date = x.Date,
                          MeetingHeld = x.MeetingHeld,
                          AgendaNumber = x.AgendaNumber,
                          AgendaItems = x.AgendaItems.Where(y => y.Worries.Count != 0).ToList()
                      }).OrderBy(x => x.AgendaNumber)
                      .ToListAsync();

            return res;
        }

        public async Task<Agenda> ExportAgenda(Guid agendaId)
        {
            var res = await ListQuery().Where(x => x.Id == agendaId)
                    .Include(x => x.AgendaItems).ThenInclude(x => x.Worries).ThenInclude(x => x.Person).ThenInclude(x => x.SspArea).AsNoTracking()
                    .Include(x => x.AgendaItems).ThenInclude(x => x.Worries).ThenInclude(x => x.Reporter).AsNoTracking()
                    .Include(x => x.AgendaItems).ThenInclude(x => x.Worries).ThenInclude(x => x.Person).ThenInclude(x => x.Worries).AsTracking()
                    .FirstAsync();

            return res;
        }

        public async Task<IEnumerable<Person>> FindPersonsInNonArchivedAgendas()
        {
            var res = await ListQuery().Where(x => !x.MeetingHeld).Include(x => x.AgendaItems)
                .ThenInclude(x => x.Worries).ThenInclude(x => x.Person).AsNoTracking()
                .SelectMany(x => x.AgendaItems.SelectMany(y => y.Worries.Select(z => z.Person))).ToListAsync();

            return res;
        }
    }
}
