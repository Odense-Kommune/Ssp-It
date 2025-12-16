using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IGroupingService : IBaseService<Grouping>
    {
        Task<IEnumerable<Grouping>> GetWithPersonGrouping();

        Task<IEnumerable<Grouping>> GetPsuWithPersonGrouping();
        Task<IEnumerable<string>> GetGroupingStats(Guid id);

        Task<IEnumerable<PersonGrouping>> GetGroupCrossRef(IEnumerable<Guid> personIds);
    }
}
