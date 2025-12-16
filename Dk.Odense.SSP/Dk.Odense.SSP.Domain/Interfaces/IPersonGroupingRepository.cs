using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IPersonGroupingRepository : IBaseRepository<PersonGrouping>
    {
        Task<bool> DeleteGroupByPersonAndGroupId(Guid personId, Guid groupId);
        void Delete(PersonGrouping group);
        Task<IEnumerable<PersonGrouping>> GetPersonsInGroup(Guid groupId);
        Task<IEnumerable<PersonGrouping>> GetGroupsForPerson(Guid personId);
        Task<IEnumerable<PersonGrouping>> GetFromGroupingId(Guid groupingId);
        Task<bool> AnyPersonGroupingFromPersonIdGroupId(Guid personId, Guid groupingId);
        Task<PersonGrouping> GetFromPersonGroup(Guid personId, Guid groupId);
    }
}
