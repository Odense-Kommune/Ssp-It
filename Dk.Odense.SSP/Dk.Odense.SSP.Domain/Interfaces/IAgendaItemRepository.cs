using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IAgendaItemRepository : IBaseRepository<AgendaItem>
    {
        Task<IEnumerable<AgendaItem>> GetAgendaItemsWithWorriesFromAgendaId(Guid agendaId);
        Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgenda(Guid agendaId);
        Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgendaNoIncludes(Guid agendaId);
        Task<AgendaItem> GetAgendaItemWithIncludes(Guid id);
    }
}
