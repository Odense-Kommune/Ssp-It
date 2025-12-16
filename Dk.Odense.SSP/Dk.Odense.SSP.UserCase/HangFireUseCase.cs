using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.ExternalServices.Interfaces;
using Dk.Odense.SSP.SagOgDokIndeks.Services;
using Dk.Odense.SSP.Sbsys.Interfaces;
using Dk.Odense.SSP.Sbsys.Model;
using Dk.Odense.SSP.Xflow.UseCase;
using Microsoft.Extensions.Options;

namespace Dk.Odense.SSP.UserCase
{
    public class HangFireUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWorryService worryService;
        private readonly IPersonService personService;
        private readonly ICprService cprService;
        private readonly ISbsysCaseService sbsysCaseService;
        private readonly IAreaRuleService areaRuleService;
        private readonly ISODIService SODIService;
        private readonly IAgendaService agendaService;
        private readonly DeleteLogicUseCase deleteLogicUseCase;
        private readonly XFlowRobustnessUseCase xflowRobustNessUseCase;
        private readonly XFlowWorryUseCase xflowWorryUseCase;
        private readonly HangfireJobConfig hangfireJobConfig;

        public HangFireUseCase(
            IUnitOfWork unitOfWork,
            IWorryService worryService,
            IPersonService personService,
            ICprService cprService,
            ISbsysCaseService sbsysCaseService,
            IAreaRuleService areaRuleService,
            IAgendaService agendaService,
            DeleteLogicUseCase deleteLogicUseCase,
            IOptions<HangfireJobConfig> hangfireJobConfig,
            ISODIService sODIService,
            XFlowRobustnessUseCase xflowRobustNessUseCase,
            XFlowWorryUseCase xflowWorryUseCase
        )
        {
            this.unitOfWork = unitOfWork;
            this.worryService = worryService;
            this.personService = personService;
            this.cprService = cprService;
            this.sbsysCaseService = sbsysCaseService;
            this.areaRuleService = areaRuleService;
            this.agendaService = agendaService;
            this.deleteLogicUseCase = deleteLogicUseCase;
            this.hangfireJobConfig = hangfireJobConfig.Value;
            SODIService = sODIService;
            this.xflowRobustNessUseCase = xflowRobustNessUseCase;
            this.xflowWorryUseCase = xflowWorryUseCase;
        }

        public string RunXFlowRobustnessesFlow()
        {
            var robustnessList = xflowRobustNessUseCase.GetNewRobustnessesXFlow().Result;
            VerifyWorries();
            if (robustnessList == null || robustnessList.Count == 0)
                return $"No new RobustnessSchemas";
            var savedIds = robustnessList.Select(x => x.Id).ToList();
            return $"Saved {robustnessList.Count} RobustnessSchemas with ids:\n{string.Join(", \n", savedIds)}";
        }

        public string RunXFlowWorryFlow()
        {
            var worryList = xflowWorryUseCase.GetNewWorriesXFlow().Result;
            VerifyWorries();
            if (worryList == null || !worryList.Any())
                return $"No new WorrySchemas";
            var savedIds = worryList.Select(x => x.Id).ToList();
            return $"Saved {worryList.Count()} WorrySchemas with ids: \n{string.Join(", \n", savedIds)}";
        }

        public void DeleteGroundless()
        {
            var groundless = worryService.GetGroundless().Result.ToList();

            foreach (var worry in groundless)
            {
                worryService.Delete(worry.Id).Wait();
            }

            unitOfWork.Commit().Wait();
        }

        public void DeleteExpiredData()
        {
            if (!hangfireJobConfig.RunCleaningProcedudre)
                return;

            try
            {
                var res = deleteLogicUseCase.DeleteLogic().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void VerifyWorries()
        {
            var toBeVerified = worryService.GetToBeVerifiedWithIncludes().Result.ToList();
            var personList = new List<Person>();
            foreach (var verify in toBeVerified)
            {
                try
                {
                    var reportedNameParts = verify.ReportedPerson.ReportedName.Split(" ");
                    var reportedNameFirst = reportedNameParts[0].Trim();
                    var reportedNameLast = reportedNameParts[^1].Trim();
                    var reportedCpr = verify.ReportedPerson.ReportedCpr?.Trim();

                    var person = personService.GetBySocialSecNum(reportedCpr).Result;
                    if (person == null)
                    {
                        if (string.IsNullOrEmpty(reportedCpr))
                        {
                            verify.PendingAutoVerify = false;
                            worryService.Update(verify);
                            unitOfWork.Commit().Wait();
                            continue;
                        }

                        var cprData = cprService.GetPerson(reportedCpr).Result;
                        if (cprData.SocialSecNum != null)
                        {
                            person = new Person()
                            {
                                SocialSecNum = cprData.SocialSecNum,
                                Name = cprData.Name,
                                Address = cprData.Address,
                                LastVerified = DateTime.Now,
                                Birthday = cprData.Birthday,
                            };
                            person = personService.Create(person).Result;
                            unitOfWork.Commit().Wait();
                            Thread.Sleep(500);
                        }

                        if (!personList.Contains(person))
                        {
                            personList.Add(person);
                        }
                    }

                    if (person != null)
                    {
                        var cprData = cprService.GetPerson(reportedCpr).Result;

                        person.Name = cprData.Name;
                        person.Address = cprData.Address;
                        if (
                            person.Name.Contains(reportedNameFirst)
                            && person.Name.Contains(reportedNameLast)
                        )
                        {
                            verify.Person_Id = person.Id;
                            verify.Approved = true;
                            verify.Groundless = null;
                        }
                    }

                    verify.PendingAutoVerify = false;
                    worryService.Update(verify);
                    if (person != null)
                    {
                        personService.Update(person);
                    }

                    unitOfWork.Commit().Wait();

                    if (person != null && personList.All(x => x.Id != person?.Id))
                    {
                        personList.Add(person);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Catch
                }
            }

            foreach (var person in personList.Where(x => x != null))
            {
                personService.UpdateSchoolData(person).Wait();
            }
        }

        public string UpdateAllSchoolData()
        {
            var persons = personService.List().Result;
            foreach (var person in persons)
            {
                personService.UpdateSchoolData(person).Wait();
            }

            return "ok";
        }

        public void UpdateSspArea()
        {
            var personList = agendaService.FindPersonsInNonArchivedAgendas().Result;
            var filteredPerons = personList.DistinctBy(person => person.Id).ToList();

            var sspAreaRules = areaRuleService.List().Result.OrderBy(x => x.Priority).ToList();

            foreach (var person in filteredPerons)
            {
                //Skal udgå
                var sbsysCases = sbsysCaseService
                    .GetCases(person.SocialSecNum.Insert(6, "-"))
                    .Result;

                var SODICases = SODIService.SearchCaseByCpr(person.SocialSecNum).Result;

                var sspAreaHit = GetRulesHit(sspAreaRules, SODICases, sbsysCases);

                if (sspAreaHit != null)
                {
                    personService.SetSspArea(person.Id, sspAreaHit.SspArea_Id).Wait();
                    unitOfWork.Commit().Wait();
                }
            }
        }

        private AreaRule GetRulesHit(
            List<AreaRule> sspAreaRules,
            IEnumerable<SODISimple> sODICases,
            SbsysCaseList sbsysCases
        )
        {
            if (sspAreaRules.Count == 0)
                return null;

            foreach (var sspAreaRule in sspAreaRules)
            {
                switch (sspAreaRule.System)
                {
                    case "DUBU":
                        if (sODICases.Any(x => x.Status != "Afsluttet" && x.ItSystem == "DUBU"))
                        {
                            return sspAreaRule;
                        }
                        break;

                    case "Momentum":
                        if (
                            sODICases.Any(x =>
                                x.Status != "Afsluttet"
                                && x.Afdeling == sspAreaRule.SearchValue?.Trim()
                                && x.ItSystem == "Momentum"
                            )
                        )
                        {
                            return sspAreaRule;
                        }
                        break;

                    case "Nexus":
                        if (
                            sODICases.Any(x =>
                                x.Status != "Afsluttet"
                                && x.Kle == sspAreaRule.SearchValue?.Trim()
                                && x.ItSystem == "Nexus"
                            )
                        )
                        {
                            return sspAreaRule;
                        }
                        break;

                    case "SBSYS":
                        if (
                            sbsysCases.Sager?.Any(x =>
                                x.Sagstilstand == "Aktiv"
                                && x.SagSkabelonId == sspAreaRule.SearchValue?.Trim()
                            ) == true
                        )
                        {
                            return sspAreaRule;
                        }
                        break;

                    default:
                        return null;
                }
            }

            return null;
        }
    }
}
