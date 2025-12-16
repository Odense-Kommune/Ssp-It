using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Application
{
    public class AgendaService : BaseService<Agenda, IAgendaRepository>, IAgendaService
    {
        public AgendaService(IAgendaRepository repository) : base(repository) { }

        public async Task<IEnumerable<Person>> GetAllPersonsOnAgenda(Guid agendaId)
        {
            return await Repository.GetAllPersonsOnAgenda(agendaId);
        }

        public async Task<IEnumerable<Agenda>> GetHeldAgendas()
        {
            return await Repository.GetHeldAgendas();
        }

        public async Task<IEnumerable<Agenda>> GetCurrentAgendasWithAgendaItems()
        {
            return await Repository.GetCurrentAgendasWithAgendaItems();
        }

        public async Task<Agenda> ExportAgenda(Guid agendaId)
        {
            return await Repository.ExportAgenda(agendaId);
        }

        public async Task<IEnumerable<Person>> FindPersonsInNonArchivedAgendas()
        {
            return await Repository.FindPersonsInNonArchivedAgendas();
        }
    }
}
