using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class AgendaItemService : BaseService<AgendaItem, IAgendaItemRepository>, IAgendaItemService
    {
        public AgendaItemService(IAgendaItemRepository repository) : base(repository)
        {
        }

        public async Task<bool> SetCategorization(Guid id, Guid categorizationId)
        {
            var entity = await Get(id);

            if (categorizationId == Guid.Empty)
            {
                entity.Categorization_Id = null;
            }
            else
            {
                entity.Categorization_Id = categorizationId;
            }

            try
            {
                Update(entity);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<IEnumerable<AgendaItem>> GetAgendaItemsWithWorriesFromAgendaId(Guid agendaId)
        {
            var res = await Repository.GetAgendaItemsWithWorriesFromAgendaId(agendaId);

            return res;
        }

        public async Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgenda(Guid agendaId)
        {
            var res = await Repository.GetExistingItemsOnAgenda(agendaId);

            return res;
        }

        public async Task<IEnumerable<AgendaItem>> GetExistingItemsOnAgendaNoIncludes(Guid agendaId)
        {
            var res = await Repository.GetExistingItemsOnAgendaNoIncludes(agendaId);

            return res;
        }

        public async Task<AgendaItem> GetAgendaItemWithIncludes(Guid id)
        {
            var res = await Repository.GetAgendaItemWithIncludes(id);

            return res;
        }
    }
}
