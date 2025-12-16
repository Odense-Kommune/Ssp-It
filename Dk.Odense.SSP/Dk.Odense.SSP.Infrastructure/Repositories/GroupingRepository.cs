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
    public class GroupingRepository : BaseRepository<Grouping>, IGroupingRepository
    {
        public GroupingRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<IEnumerable<Grouping>> GetWithPersonGrouping()
        {
            var res = await ListQuery().Include(x => x.PersonGroupings).Where(x => x.Type == "grouping").AsNoTracking()
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<Grouping>> GetPsuWithPersonGrouping()
        {
            var res = await ListQuery().Include(x => x.PersonGroupings).Where(x => x.Type == "psu").AsNoTracking()
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<string>> GetGroupingStats(Guid id)
        {
            var res = await ListQuery()
                    .Where(p => p.Id == id)
                    .SelectMany(p => p.PersonGroupings)
                    .Select(pg => pg.Person)
                    .Distinct()
                    .SelectMany(person => DatabaseContext.Worries
                        .Where(w => w.Person_Id == person.Id)
                        .Select(w => w.AgendaItem)
                        .Where(a => a.Categorization_Id != null)
                        .OrderByDescending(a => a.Agenda.Date)
                        .Select(a => a.Categorization.Value)
                        .Take(1))
                    .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<PersonGrouping>> GetCrossRef(IEnumerable<Guid> ids)
        {
            try
            {
                var persons = await DatabaseContext.Persons
                .Where(x => ids.Contains(x.Id))
                .Include(x => x.PersonGroupings)
                    .ThenInclude(x => x.Grouping)
                .Include(x => x.SchoolData)
                .Include(x => x.Worries)
                .Include(x => x.SspArea)
                .AsNoTracking()
                .ToListAsync();

                var res = persons.SelectMany(p => p.PersonGroupings).ToList();

                return res;
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public override Grouping Update(Grouping value)
        {
            var entity = Get(value.Id).Result;
            entity.Value = value.Value;

            var dbEntity = DbSet.Attach(entity);

            DbSet.Update(entity);
            dbEntity.State = EntityState.Modified;

            return entity;
        }
    }
}