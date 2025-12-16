using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class PersonGroupingRepository : BaseRepository<PersonGrouping>, IPersonGroupingRepository
    {
        public PersonGroupingRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public void Delete(PersonGrouping group)
        {
            if (group != null)
                DbSet.Remove(group);
        }

        public async Task<IEnumerable<PersonGrouping>> GetPersonsInGroup(Guid groupId)
        {
            var res = await DbSet
                .Where(x => x.Grouping_Id == groupId)
                .Include(x => x.Person)
                    .ThenInclude(x=> x.PersonGroupings)
                    .ThenInclude(x => x.Grouping)

                .Include(x => x.Person)
                    .ThenInclude(x => x.PersonGroupings)
                    .ThenInclude(x => x.Classification)               
                
                .AsTracking()
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<PersonGrouping>> GetGroupsForPerson(Guid personId)
        {
            var res = await ListQuery().Where(x => x.Person_Id == personId).Include(x => x.Grouping).AsNoTracking().ToListAsync();

            return res;
        }

        public async Task<IEnumerable<PersonGrouping>> GetFromGroupingId(Guid groupingId)
        {
            var res = await ListQuery().Where(x => x.Grouping_Id == groupingId).ToListAsync();

            return res;
        }

        public async Task<bool> AnyPersonGroupingFromPersonIdGroupId(Guid personId, Guid groupingId)
        {
            var res = await ListQuery().AnyAsync(x => x.Person_Id == personId && x.Grouping_Id == groupingId);

            return res;
        }

        public async Task<PersonGrouping> GetFromPersonGroup(Guid personId, Guid groupId)
        {
            var res = await ListQuery().FirstOrDefaultAsync(x =>
                x.Person_Id == personId && x.Grouping_Id == groupId);

            return res;
        }

        public async Task<bool> DeleteGroupByPersonAndGroupId(Guid personId, Guid groupId)
        {
            var grouping = await DbSet.FirstOrDefaultAsync(x => x.Person_Id == personId && x.Grouping_Id == groupId);
            if (grouping == null) return false;
            DbSet.Remove(grouping);
            return true;

        }
    }
}
