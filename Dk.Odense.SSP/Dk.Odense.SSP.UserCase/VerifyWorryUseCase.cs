using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.ExternalServices.Interfaces;
using Dk.Odense.SSP.UserCase.ViewModel;
using Microsoft.Extensions.Options;

namespace Dk.Odense.SSP.UserCase
{
    public class VerifyWorryUseCase
    {
        private readonly ICprService cprService;
        private readonly IPersonService personService;
        private readonly IReportedPersonService reportedPersonService;
        private readonly IWorryService worryService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAgendaItemService agendaItemService;
        private readonly DevelopmentSettingsConfig _devConfig;

        public VerifyWorryUseCase(
            ICprService cprService,
            IPersonService personService,
            IReportedPersonService reportedPersonService,
            IWorryService worryService,
            IAgendaItemService agendaItemService,
            IOptions<DevelopmentSettingsConfig> devConfig,
            IUnitOfWork unitOfWork
        )
        {
            this.cprService = cprService;
            this.personService = personService;
            this.reportedPersonService = reportedPersonService;
            this.worryService = worryService;
            this.agendaItemService = agendaItemService;
            this._devConfig = devConfig.Value;
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> GetNumberOfPendingWorries()
        {
            return (await worryService.GetToBeVerified()).Count();
        }

        public async Task<VerifyWorry> GetVerifyWorry(Guid id)
        {
            var res = await worryService.GetWithIncludes(id);

            var ret = new VerifyWorry()
            {
                SocialSecNum = res?.Person?.SocialSecNum,
                Assessment = res?.Assessment,
                Reporter = res?.Reporter,
                PoliceWorryCategory = res?.PoliceWorryCategory,
                PoliceWorryRole = res?.PoliceWorryRole,
                ReportedPerson = res?.ReportedPerson,
                Concern = res?.Concern,
            };
            return ret;
        }

        public async Task<IEnumerable<VerifyWorryMenuItem>> GetMenuItems()
        {
            var res = (await worryService.GetNotPendingWithIncludes())
                .Select(x => new VerifyWorryMenuItem()
                {
                    Id = x.Id,
                    Source = x.Reporter.Workplace,
                    Increment = x.Increment,
                    Groundless = x.Groundless != null,
                    Approved = x.Approved,
                    ReportedPerson = x.ReportedPerson,
                    SocialSecData =
                        x.Person?.SocialSecNum != null
                            ? new NavneOpslagData()
                            {
                                Name = x.Person.Name,
                                Address = x.Person.Address,
                                SocialSecNum = x.Person.SocialSecNum,
                            }
                            : new NavneOpslagData(),
                })
                .ToList();

            return res;
        }

        public async Task<bool> SetGroundless(Guid id)
        {
            await worryService.SetGroundless(id);

            try
            {
                await unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SetApproved(Guid id, string socialSecNum)
        {
            var ret = await ApproveWorry(id, socialSecNum);
            try
            {
                await unitOfWork.Commit();
                Person person = await GetCreatePerson(socialSecNum); // (Also creates dummy data for unknown CPR in dev environment)
                await UpdateSocialSecData(person); //Doesn't change dummy data

                await personService.UpdateSchoolData(person);
                await unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return false;
            }

            return ret;
        }

        public async Task<bool> Unverify(Guid id)
        {
            var ret = await worryService.Unverify(id);

            try
            {
                await unitOfWork.Commit();
                await AgendaItemCleanup(id);
                await unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return false;
            }

            return ret;
        }

        #region Private methods
        private async Task AgendaItemCleanup(Guid id)
        {
            var agendaItemId = await worryService.AgendaItemCleanup(id);

            if (agendaItemId == Guid.Empty)
                return;

            await agendaItemService.Delete(agendaItemId);
        }

        private async Task<bool> ApproveWorry(Guid id, string socialSecNum)
        {
            var entity = await worryService.Get(id);

            if (socialSecNum != "")
            {
                var person = await GetCreatePerson(socialSecNum);
                if (person.SocialSecNum == null)
                    return false;

                entity.Person_Id = person.Id;
            }

            if (entity.Person_Id == null)
                return false;

            entity.Approved = true;
            entity.Groundless = null;

            worryService.Update(entity);

            return true;
        }

        private async Task<Person> GetCreatePerson(string socialSecNum)
        {
            //If the person already exists, just return that one
            Person person = await personService.GetBySocialSecNum(socialSecNum);

            //If the person doesn't exist, create a new one
            if (person == null)
            {
                if (_devConfig.IsDevelopment)
                {
                    // To avoid errors in development from non-existing CPR numbers (and for data security),
                    // we don't check the CPR service, but instead create a dummy person. We don't want our
                    // debug Person-objects to accidentally get info from real people.
                    return await CreateDummyPerson(socialSecNum);
                }
                else
                {
                    NavneOpslagData cprData =
                        await cprService.GetPerson(socialSecNum)
                        ?? throw new Exception("Person ikke fundet");
                    return await personService.Create(
                        new Person()
                        {
                            Name = cprData.Name,
                            Address = cprData.Address,
                            Birthday = cprData.Birthday,
                            SocialSecNum = socialSecNum,
                        }
                    );
                }
            }

            return person;
        }

        private async Task<Person> CreateDummyPerson(string socialSecNum)
        {
            ReportedPerson reportedPerson = await reportedPersonService.GetBySocialSecNum(
                socialSecNum
            );
            return await personService.Create(
                new Person()
                {
                    Name = reportedPerson.ReportedName + " (test)",
                    Address = reportedPerson.ReportedAdress + " (test)",
                    Birthday = CreateDummyDateFromCpr(socialSecNum),
                    SocialSecNum = socialSecNum,
                }
            );
        }

        private string CreateDummyDateFromCpr(string socialSecNum)
        {
            string day = socialSecNum.Substring(0, 2);
            string month = socialSecNum.Substring(2, 2);
            string year = socialSecNum.Substring(4, 2);
            string currentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            year = (int.Parse(year) > int.Parse(currentYear)) ? "19" + year : "20" + year;
            return year + month + day;
        }

        private async Task UpdateSocialSecData(Person person)
        {
            var navneOpslag = await cprService.GetPerson(person.SocialSecNum);
            if (navneOpslag.SocialSecNum == null)
            {
                return;
            }

            person.Name = navneOpslag.Name;
            person.Address = navneOpslag.Address;
            person.Birthday = navneOpslag.Birthday;
            person.LastVerified = DateTime.Now;
            personService.Update(person);
        }
        #endregion
    }
}
