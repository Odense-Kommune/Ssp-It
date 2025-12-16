using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class GroupingService : BaseService<Grouping, IGroupingRepository>, IGroupingService
    {
        public GroupingService(IGroupingRepository repository) : base(repository)
        {
        }

        public async Task<IEnumerable<Grouping>> GetWithPersonGrouping()
        {
            return await Repository.GetWithPersonGrouping();
        }

        public async Task<IEnumerable<Grouping>> GetPsuWithPersonGrouping()
        {
            return await Repository.GetPsuWithPersonGrouping();
        }

        public async Task<IEnumerable<string>> GetGroupingStats(Guid id)
        {
            return await Repository.GetGroupingStats(id);
        }

        public async Task<IEnumerable<PersonGrouping>> GetGroupCrossRef(IEnumerable<Guid> personIds)
        {
            return await Repository.GetCrossRef(personIds);
        }
    }
}
