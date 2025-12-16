using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.ExternalServices.Interfaces;

namespace Dk.Odense.SSP.Application
{
    public class PersonService : BaseService<Person, IPersonRepository>, IPersonService
    {
        private readonly ISchoolAreaService schoolAreaService;
        private readonly ISchoolDataService schoolDataService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICategorizationService categorizationService;

        public PersonService(IPersonRepository repository, ISchoolAreaService schoolAreaService, ISchoolDataService schoolDataService, IUnitOfWork unitOfWork, ICategorizationService categorizationService) : base(repository)
        {
            this.schoolAreaService = schoolAreaService;
            this.schoolDataService = schoolDataService;
            this.unitOfWork = unitOfWork;
            this.categorizationService = categorizationService;
        }

        public async Task<Person> GetBySocialSecNum(string socialSecNum)
        {
            return await Repository.GetBySocialSecNum(socialSecNum);
        }

        public void SetSspStopDate(Guid id, DateTime date)
        {
            Repository.SetSspStopDate(new Person() { Id = id, SspStopDate = date });
        }

        public async Task SetSspArea(Guid id, Guid sspAreaId)
        {
            var entity = await Get(id);

            if (sspAreaId != Guid.Empty)
            {
                entity.SspArea_Id = sspAreaId;
            }
            else
            {
                entity.SspArea_Id = null;
            }

            Update(entity);
        }

        public void DeleteSspStopDate(Guid id)
        {
            Repository.SetSspStopDate(new Person() { Id = id, SspStopDate = null });
        }

        public async Task<IEnumerable<DeletePerson>> GetPersonsForDeleting(DateTime date)
        {
            try
            {
                // TODO: Er det max med DeleteAfterSspEnd == true?
                var maxCatExpire = (await categorizationService.GetValidList()).OrderByDescending(x => x.DaysToExpire)
                .First().DaysToExpire;

                var allPersonList = await Repository.GetPersonsForDeleting();

                var personsForDeleting = allPersonList.ToList();

                // Persons with Categorization And SspStopDate
                var deleteAfterSspEnd = personsForDeleting.Where(x => (x.DelteAfterSspEnd != null && x.DelteAfterSspEnd == true) &&
                                                                               (x.LatestCategorizationDeleteAfter != null && x.SspStopDate != null &&
                                                                                x.LatestWorryDate < date.AddDays(-(int)x.LatestCategorizationDeleteAfter)) &&
                                                                               (x.SspStopDate < date.AddDays(-(int)x.LatestCategorizationDeleteAfter))).ToList();
                // Persons with No SspStopDate With Categorization
                var deleteNoSspDate = personsForDeleting.Where(x => x.DelteAfterSspEnd == false &&
                                                                    (x.LatestCategorizationDeleteAfter != null &&
                                                                     x.LatestWorryDate <
                                                                     date.AddDays(
                                                                         -(int)x.LatestCategorizationDeleteAfter))).ToList();

                // Persons without SspStopDate and categorization
                var deleteNoCategorization = personsForDeleting.Where(x =>
                                                                           (x.LatestCategorizationDeleteAfter == null &&
                                                                            x.LatestWorryDate <
                                                                            date.AddDays(
                                                                                -maxCatExpire))).ToList();

                deleteAfterSspEnd.AddRange(deleteNoSspDate);
                deleteAfterSspEnd.AddRange(deleteNoCategorization);

                var ret = deleteAfterSspEnd.Distinct().ToList();

                return deleteAfterSspEnd;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SetSocialWorker(Guid id, string socialWorker)
        {
            var entity = await Get(id);
            entity.SocialWorker = socialWorker;
            Repository.Update(entity);
        }

        public async Task<Categorization> GetLatestCategorization(Guid id)
        {
            var ret = await Repository.GetLatestCategorization(id);

            return ret?.AgendaItem.Categorization;
        }

        public async Task<InternalSchoolData> GetSchoolData(Guid personId)
        {
            return await Repository.GetSchoolData(personId);
        }

        public async Task UpdateSchoolData(Person person)
        {
            try
            {
                var newSchooldata = await schoolAreaService.GetSchoolData(person.SocialSecNum, "Hangfire");

                if (person.SchoolData_Id == null)
                {
                    var schoolData = await schoolDataService.Create(
                        new InternalSchoolData()
                        {
                            Name = newSchooldata.Name,
                            DateFirst = newSchooldata.DateFirst,
                            DateLast = newSchooldata.DateLast
                        });

                    person.SchoolData_Id = schoolData.Id;
                    Update(person);
                }
                else
                {
                    var schoolData = await schoolDataService.Get((Guid)person.SchoolData_Id);

                    schoolData.Name = newSchooldata.Name;

                    schoolData.DateFirst = newSchooldata.DateFirst;
                    schoolData.DateLast = newSchooldata.DateLast;

                    schoolDataService.Update(schoolData);
                }

                await unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<Person>> GetPersonWithPendingWorries()
        {
            return await Repository.GetPersonWithPendingWorries();
        }

        public async Task<Person> GetPersonWithIncludes(Guid id)
        {
            return await Repository.GetPersonWithIncludes(id);
        }

        public async Task<IEnumerable<Person>> GetPersonsWithIncludes()
        {
            return await Repository.GetPersonsWithIncludes();
        }

        public async Task<IEnumerable<Person>> SearchCpr(string term)
        {
            return await Repository.SearchCpr(term);
        }

        public async Task<IEnumerable<Person>> SearchGroupAndName(string term)
        {
            return await Repository.SearchGroupAndName(term);
        }
    }
}
