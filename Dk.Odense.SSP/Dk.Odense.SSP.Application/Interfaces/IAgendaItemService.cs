using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IAgendaItemService : IBaseService<AgendaItem>
    {
        Task<bool> SetCategorization(Guid id, Guid categorizationId);
        Task<IEnumerable<AgendaItem>> GetAgendaItemsWithWorriesFromAgendaId(Guid agendaId);
        Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgenda(Guid agendaId);
        Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgendaNoIncludes(Guid agendaId);
        Task<AgendaItem> GetAgendaItemWithIncludes(Guid id);
    }
}
