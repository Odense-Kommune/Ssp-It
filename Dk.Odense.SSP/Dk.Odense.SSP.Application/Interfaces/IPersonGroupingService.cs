using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IPersonGroupingService : IBaseService<PersonGrouping>
    {
        Task<IEnumerable<PersonGrouping>> GetPersonsInGroup(Guid groupId);
        Task<IEnumerable<PersonGrouping>> GetGroupsForPerson(Guid personId);
        Task<PersonGrouping> GetFromPersonGroup(Guid personId, Guid groupId);
        Task<bool> DeleteGroupByPersonAndGroupId(Guid personId, Guid groupId);
        Task<IEnumerable<PersonGrouping>> GetFromGroupingId(Guid groupingId);
        Task<bool> AnyPersonGroupingFromPersonIdGroupId(Guid personId, Guid groupingId);
        void Delete(PersonGrouping group);
    }
}
