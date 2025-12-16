using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class PersonGroupingService : BaseService<PersonGrouping, IPersonGroupingRepository>, IPersonGroupingService
    {
        public PersonGroupingService(IPersonGroupingRepository repository) : base(repository)
        {
        }

        public async Task<IEnumerable<PersonGrouping>> GetGroupsForPerson(Guid personId)
        {
            return await Repository.GetGroupsForPerson(personId);
        }

        public async Task<PersonGrouping> GetFromPersonGroup(Guid personId, Guid groupId)
        {
            return await Repository.GetFromPersonGroup(personId, groupId);
        }

        public async Task<IEnumerable<PersonGrouping>> GetPersonsInGroup(Guid groupId)
        {
            return await Repository.GetPersonsInGroup(groupId);
        }

        public Task<bool> DeleteGroupByPersonAndGroupId(Guid personId, Guid groupId)
        {
            return Repository.DeleteGroupByPersonAndGroupId(personId, groupId);
        }

        public async Task<bool> AnyPersonGroupingFromPersonIdGroupId(Guid personId, Guid groupingId)
        {
            return await Repository.AnyPersonGroupingFromPersonIdGroupId(personId, groupingId);
        }

        public async Task<IEnumerable<PersonGrouping>> GetFromGroupingId(Guid groupingId)
        {
            return await Repository.GetFromGroupingId(groupingId);
        }

        public void Delete(PersonGrouping group)
        {
            Repository.Delete(group);
        }
    }
}
