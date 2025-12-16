using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IAgendaRepository : IBaseQueryRepository<Agenda>
    {
        Task<IEnumerable<Person>> GetAllPersonsOnAgenda(Guid agendaId);
        Task<IEnumerable<Agenda>> GetHeldAgendas();
        Task<IEnumerable<Agenda>> GetCurrentAgendasWithAgendaItems();
        Task<Agenda> ExportAgenda(Guid agendaId);
        Task<IEnumerable<Person>> FindPersonsInNonArchivedAgendas();
    }
}
