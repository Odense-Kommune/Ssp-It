using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<Person> GetBySocialSecNum(string socialSecNum)
        {
            return await ListQuery().FirstOrDefaultAsync(x => x.SocialSecNum == socialSecNum);
        }

        public async Task<InternalSchoolData> GetSchoolData(Guid personId)
        {
            var x = await ListQuery().Where(q => q.Id == personId).Include(q => q.SchoolData).AsNoTracking().FirstOrDefaultAsync();
            if (x?.SchoolData == null)
                return null;
            x.SchoolData.Person = null;
            return x.SchoolData;
        }

        public async Task<IEnumerable<Person>> GetPersonWithPendingWorries()
        {
            var res = await ListQuery().Where(x => x.Worries.Any(y => y.AgendaItem_Id == null && y.Approved)).Include(x => x.Worries).AsNoTracking().Select(x => new Person()
            {
                Id = x.Id,
                SchoolData_Id = x.SchoolData_Id,
                Worries = x.Worries.Where(y => y.AgendaItem_Id == null && y.Approved).ToList(),
                Name = x.Name,
                Address = x.Address,
                Birthday = x.Birthday,
                LastVerified = x.LastVerified,
                SocialSecNum = x.SocialSecNum,
                SspArea_Id = x.SspArea_Id,
                SspStopDate = x.SspStopDate
            }).ToListAsync();

            return res;
        }

        public async Task<Worry> GetLatestCategorization(Guid id)
        {
            var res = await ListQuery().Where(x => x.Id == id)
                .SelectMany(x => x.Worries)
                .Where(x => x.AgendaItem.Categorization_Id != null)
                .Include(x => x.AgendaItem)
                .ThenInclude(x => x.Categorization).AsNoTracking()
                .OrderByDescending(x => x.AgendaItem.Agenda.Date)
                .FirstOrDefaultAsync();

            return res;
        }

        public async Task<Person> GetPersonWithIncludes(Guid id)
        {
            var res = await ListQuery()
                .Where(x => x.Id == id)
                .Include(x => x.Worries)
                .ThenInclude(x => x.AgendaItem)
                .ThenInclude(x => x.Agenda)
                .Include(x => x.Worries)
                .ThenInclude(x => x.AgendaItem)
                .ThenInclude(x => x.Categorization)
                .Include(x => x.PersonGroupings).ThenInclude(x => x.Grouping).AsNoTracking()
                .Include(x => x.PersonGroupings).ThenInclude(x => x.Classification).AsNoTracking()
                .Include(x => x.SspArea).AsNoTracking()
                .Include(x => x.SchoolData).AsNoTracking().FirstOrDefaultAsync();

            return res;
        }

        public async Task<IEnumerable<Person>> GetPersonsWithIncludes()
        {
            var res = await ListQuery().Include(x => x.Worries)
                .ThenInclude(x => x.AgendaItem)
                .ThenInclude(x => x.Agenda)
                .Include(x => x.Worries)
                .ThenInclude(x => x.AgendaItem)
                .ThenInclude(x => x.Categorization)
                .Include(x => x.PersonGroupings)
                .ThenInclude(x => x.Grouping).AsNoTracking()
                .Include(x=>x.PersonGroupings)
                .ThenInclude(x=>x.Classification).AsNoTracking()
                .Include(x => x.SspArea).AsNoTracking()
                .Include(x => x.SchoolData).AsNoTracking()
                
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<Person>> SearchCpr(string term)
        {
            var res = await ListQuery().Where(p => p.SocialSecNum.Contains(term)).ToListAsync();

            return res;
        }

        public async Task<IEnumerable<Person>> SearchGroupAndName(string term)
        {
            var lowTerm = term.ToLower();

            var res = await ListQuery().Where(p => p.PersonGroupings.Any(a => a.Grouping.Value.ToLower().Contains(lowTerm)) || p.Name.ToLower().Contains(lowTerm)).ToListAsync();

            return res;
        }

        public async Task<IEnumerable<DeletePerson>> GetPersonsForDeleting()
        {
            try
            {
                var res = await ListQuery()
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.AgendaItem)
                    .ThenInclude(x => x.Agenda)
                    .Include(x => x.Worries)
                    .ThenInclude(x => x.AgendaItem)
                    .ThenInclude(x => x.Categorization)

                .Select(x => new DeletePerson()
                {
                    Id = x.Id,
                    Name = x.Name,
                    WorryCount = x.Worries.Count,
                    SspStopDate = x.SspStopDate,
                    DelteAfterSspEnd = x.Worries.Where(y => y.AgendaItem_Id != null && y.AgendaItem.Categorization_Id != null).OrderByDescending(y => y.AgendaItem.Agenda.Date).FirstOrDefault().AgendaItem.Categorization.DeleteAfterSspEnd,
                    LatestWorryDate = x.Worries.OrderByDescending(y => y.CreatedDate).Single().CreatedDate,
                    LatestCategorizationDeleteAfter = x.Worries.Where(y => y.AgendaItem_Id != null && y.AgendaItem.Categorization_Id != null).OrderByDescending(y => y.AgendaItem.Agenda.Date).First().AgendaItem.Categorization.DaysToExpire
                }).ToListAsync();

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SetSspStopDate(Person person)
        {
            DbSet.Attach(person);
            DatabaseContext.Entry(person).Property("SspStopDate").IsModified = true;
        }
    }
}
