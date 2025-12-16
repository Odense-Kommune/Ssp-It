using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.UserCase.ViewModel;

namespace Dk.Odense.SSP.UserCase
{
    public class PersonUseCase
    {
        private readonly IPersonService personService;
        private readonly IUnitOfWork unitOfWork;

        public PersonUseCase(IPersonService personService, IUnitOfWork unitOfWork)
        {
            this.personService = personService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<PersonDto> Get(Guid id)
        {
            var entity = await personService.GetPersonWithIncludes(id);

            return MapPerson(entity);
        }

        public async Task<IEnumerable<Person>> SearchCpr(string term)
        {
            var personList = await personService.GetPersonsWithIncludes();
            return personList;
        }

        public async Task<IEnumerable<Person>> SearchGroupAndName(string term)
        {
            var lowTerm = term.ToLower();
            var persons = await personService.SearchGroupAndName(lowTerm);
            return persons;
        }

        public async Task<IEnumerable<PersonDto>> List()
        {
            var res = (await personService.GetPersonsWithIncludes()).Select(x => MapPerson(x));

            return res;
        }

        public async Task<bool> SetSspArea(Guid id, Guid sspAreaId)
        {
            try
            {
                await personService.SetSspArea(id, sspAreaId);
                await unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteSspStopDate(Guid id)
        {
            try
            {
                personService.DeleteSspStopDate(id);

                await unitOfWork.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }


        #region Private Methods
        public PersonDto MapPerson(Person x, Guid? agendaId = null)
        {
            var schooldata = new SchoolDataDTO();
            if (x.SchoolData != null)
            {
                schooldata.Name = x.SchoolData.Name;
                schooldata.DateFirst = x.SchoolData.DateFirst;
                schooldata.DateLast = x.SchoolData.DateLast;
            }

            var res = new PersonDto()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                SocialSecNum = x.SocialSecNum,
                SocialWorker = x.SocialWorker,
                Age = GetAge(x.Birthday),
                WorriesCount = x.Worries.Count,
                Groupings =  x.PersonGroupings.Select(MapPersonGrouping).ToList(),
                SspArea = MapSspArea(x.SspArea),
                Categorization = MapCategorization(x.Worries.Where(y => y.AgendaItem_Id != null && y.AgendaItem.Categorization_Id != null).OrderByDescending(y => y.AgendaItem?.Agenda?.Date).FirstOrDefault()?.AgendaItem?.Categorization),
                AgendaCategorization = agendaId == null ? null : MapCategorization(x.Worries.Where(y => y?.AgendaItem?.Agenda?.Id == agendaId).OrderBy(y => y.AgendaItem.Agenda?.Date).FirstOrDefault()?.AgendaItem.Categorization),
                SchoolData = schooldata,
                SspStopDate = x.SspStopDate,
                WorryIncrements = x.Worries.Select(y => y.Increment)
            };

            return res;
        }

        private GuidString MapCategorization(Categorization categorization)
        {
            return categorization == null ? new GuidString() : new GuidString() { Id = categorization.Id, Value = categorization.Value, DeleteAfterSsp = categorization.DeleteAfterSspEnd };
        }

        private Grouping MapPersonGrouping(PersonGrouping x)
        {
            return new Grouping()
            {
                Id = x.Grouping_Id,
                Value = x.Grouping.Value,
                Type = x.Grouping.Type,
                ClassificationName = x.Classification?.Value
            };
        }

        private GuidString MapSspArea(SspArea sspArea)
        {
            return sspArea == null ? new GuidString() : new GuidString() { Id = sspArea.Id, Value = sspArea.Value };
        }
        #endregion

        public async Task<bool> SetSspStopDate(Guid id, DateTime date)
        {
            try
            {
                personService.SetSspStopDate(id, date);
                await unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<Categorization> GetLatestCategorization(Guid id)
        {
            return await personService.GetLatestCategorization(id);
        }

        private string GetAge(string birthday)
        {
            if(birthday != null)
            {
                var bornDay = Int32.Parse(birthday.Substring(6, 2));
                var bornMonth = Int32.Parse(birthday.Substring(4, 2));
                var bornYear = Int32.Parse(birthday.Substring(0, 4));

                var born = new DateTime(bornYear, bornMonth, bornDay);

                var span = DateTime.Now.Date - born;

                var age = (new DateTime(1, 1, 1) + span).Year - 1;

                return age.ToString();
            }
            else return "Fejl ved udlæsning";
        }

        public async Task<IEnumerable<DeletePerson>> GetPersonForDelete(DateTime date)
        {
            return await personService.GetPersonsForDeleting(date);
        }

        public async Task<bool> SetSocialWorker(Guid id, string socialWorker)
        {
            try
            {
                await personService.SetSocialWorker(id, socialWorker);
                await unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
