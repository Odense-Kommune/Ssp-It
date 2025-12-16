using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Xflow.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using static Dk.Odense.SSP.Core.Enum;

namespace Dk.Odense.SSP.Xflow.Services
{
    public class XFlowWorryService : BaseXFlowService<XFlowWorry, IXFlowWorryRepository>, IXFlowWorryService
    {
        private readonly IXFlowWorryRepository _repository;
        private readonly XFlowConfig _xFlowConfig;
        private readonly HttpClient _httpClient;

        private readonly string[] publicId;
        private readonly string reportedPersonFormName;
        private readonly string worryFormName;
        private readonly string assessmentFormName;
        private readonly string reporterFormName;

        public XFlowWorryService(IXFlowWorryRepository repository, IOptions<XFlowConfig> xFlowConfig) : base(repository, xFlowConfig)
        {
            _repository = repository;
            _xFlowConfig = xFlowConfig.Value;
            _httpClient = CreateClient();

            // Set form variables
            publicId = [_xFlowConfig.WorrySchemaId];
            reportedPersonFormName = _xFlowConfig.WorryReportedPersonFormName;
            worryFormName = _xFlowConfig.WorryFormName;
            assessmentFormName = _xFlowConfig.WorryAssessmentFormName;
            reporterFormName = _xFlowConfig.WorryReporterFormName;
        }

        public async Task<List<XFlowWorry>?> GetNewWorryIds()
        {
            IEnumerable<JToken>? formIds = await GetFormIds(publicId);
            if (formIds != null && formIds.ToList().Count > 0)
            {
                var idList = new List<XFlowWorry>();
                foreach (var formId in formIds)
                {
                    var publicId = formId.Value<string>("publicId");
                    if (publicId == null) continue;
                    var guid = Guid.Parse(publicId);
                    var result = await GetFormsFromXFlow(guid);
                    var worry = new XFlowWorry()
                    {
                        Id = guid,
                        Value = result?.ToString()
                    };
                    bool alreadyExists = await FindDuplicate(guid);
                    if (!alreadyExists)
                    {
                        idList.Add(worry);
                    }
                }
                return idList;
            }
            return null;
        }

        public Worry? MapNewWorry(XFlowWorry xFlowWorry)
        {
            if (xFlowWorry != null)
            {
                var worry = new Worry();
                var formData = JToken.Parse(xFlowWorry.Value);
                if (formData != null) worry = MapFormGroup(formData, worry);
                if (worry != null) return worry;
            }
            return null;
        }

        //Can be moved to BaseService
        public bool IsValidForm(XFlowWorry xFlowWorry)
        {
            if(xFlowWorry != null)
            {
                var formData = JToken.Parse(xFlowWorry.Value);
                if (formData != null)
                {
                    var status = formData.Value<string>("status");
                    return status == "Afsluttet";
                } 
            }
            return false;
        }

        private Worry? MapFormGroup(JToken formGroup, Worry worry)
        {
            var forms = formGroup.Value<JArray>("blanketter");
            if (forms == null) return null;
            foreach (var form in forms)
            {
                ReportedPerson? reportedPerson = null;
                Concern? concern = null;
                Assessment? assessment = null;
                Reporter? reporter = null;

                var formName = form.Value<string>("blanketnavn");
                if (formName == null) continue; // Skip if formName is null

                switch (formName)
                {
                    case var name when name.Equals(reportedPersonFormName):
                        reportedPerson = MapReportedPerson(form);
                        break;
                    case var name when name.Equals(worryFormName):
                        concern = MapConcern(form);
                        break;
                    case var name when name.Equals(assessmentFormName):
                        assessment = MapAssessment(form);
                        break;
                    case var name when name.Equals(reporterFormName):
                        reporter = MapReporter(form);
                        break;
                    default:
                        break;
                }

                if (reportedPerson != null)
                {
                    worry.ReportedPerson_Id = reportedPerson.Id;
                    worry.ReportedPerson = reportedPerson;
                }
                if(concern != null)
                {
                    worry.Concern_Id = concern.Id;
                    worry.Concern = concern;
                }
                if (assessment != null)
                {
                    worry.Assessment_Id = assessment.Id;
                    worry.Assessment = assessment;
                }
                if (reporter != null)
                {
                    worry.Reporter_Id = reporter.Id;
                    worry.Reporter = reporter;
                }
            }
            return worry;
        }

        private ReportedPerson? MapReportedPerson(JToken form)
        {
            var elements = form.Value<JArray>("elementer");
            var elementPerson = elements?.FirstOrDefault(e => e.Value<string>("identifier") == "ElementPerson");
            var values = elementPerson?.Value<JObject>("values");
            var elementMultiSelect = elements?.FirstOrDefault(e => e.Value<string>("identifier") == "ElementMultiSelect");
            if (elementPerson != null && values != null)
            {
                string? firstName = values.Value<string>("Fornavn");
                string? lastName = values.Value<string>("Efternavn");

                //refactor later
                var address = elementMultiSelect?
                    .Value<JArray>("children")?.FirstOrDefault(x => x.Value<string>("identifier") == "ElementContainer")?
                    .Value<JArray>("children")?.FirstOrDefault(y => y.Value<string>("identifier") == "ElementAdresse")?
                    .Value<JObject>("values")?.Value<string>("Adresse");
                var socialSecurityNumber = elementMultiSelect?
                    .Value<JArray>("children")?.FirstOrDefault(x => x.Value<string>("identifier") == "ElementContainer-1")?
                    .Value<JArray>("children")?.FirstOrDefault(y => y.Value<string>("identifier") == "ElementCPR")?
                    .Value<JObject>("values")?.Value<string>("CPRNummer");

                var reportedPerson = new ReportedPerson()
                {
                    ReportedName = ReplaceChars(firstName + " " + lastName),
                    ReportedCpr = ReplaceChars(FormatSocialSecNum(socialSecurityNumber)),
                    ReportedAdress = ReplaceChars(address)
                };
                return reportedPerson;
            }
            return null;
        }

        private Concern MapConcern(JToken form)
        {
            var elements = form.Value<JArray>("elementer");
            var crimeConcern = GetChildFromField(elements, "CrimeConcern", "RenTekst");
            var reportedToPolice = elements?.FirstOrDefault(x => x.Value<string>("identifier") == "ReportedToPolice");
            var notifyConcern = elements?.FirstOrDefault(x => x.Value<string>("identifier") == "NotifyConcern");
            var concern = new Concern()
            {
                CrimeConcern = ReplaceChars(crimeConcern),
                ReportedToPolice = MapEnumAnswer(reportedToPolice),
                NotifyConcern = MapEnumAnswer(notifyConcern)
            };
            return concern;
        }

        private static Answer MapEnumAnswer(JToken? element)
        {
            var value = element?.Value<JObject>("values")?.Value<string>("MultiSelectTekst") ?? ""; //Should never return an empty string, but if so, maps to yellow
            return value switch
            {
                "Ja" => Answer.Yes,
                "Nej" => Answer.No,
                "Ved ikke" => Answer.DontKnow,
                _ => Answer.DontKnow,
            };
        }

        private Reporter? MapReporter(JToken form)
        {
            var elementMultiSelectChildren = form.Value<JArray>("elementer")?
                .FirstOrDefault(x => x.Value<string>("identifier") == "ElementMultiSelect")?.Value<JArray>("children");
            var personContainerChildren = elementMultiSelectChildren?
                .FirstOrDefault(y => y.Value<string>("identifier") == "ElementContainer")?.Value<JArray>("children");
            var institutionContainerChildren = elementMultiSelectChildren?
                .FirstOrDefault(z => z.Value<string>("identifier") == "ElementContainer-1")?.Value<JArray>("children");

            //if "privatperson"
            if (personContainerChildren != null)
            {
                return MapReporterPerson(personContainerChildren);
            }
            //if "institution"
            else if (institutionContainerChildren != null)
            {
                return MapReporterInstitution(institutionContainerChildren);
            }
            return null;
        }

        //Note: this is not a typo. It's a reporter who is a person. I thought I named it wrong.
        private Reporter? MapReporterPerson(JToken? personContainerChildren)
        {
            var elementPerson = personContainerChildren?.FirstOrDefault(z => z.Value<string>("identifier") == "ElementPerson");
            var personValues = elementPerson?.Value<JObject>("values");
            if (elementPerson == null || personValues == null) return null;
            string? firstName = personValues.Value<string>("Fornavn");
            string? lastName = personValues.Value<string>("Efternavn");
            string? phoneNumber = GetChildFromField(personContainerChildren, "ElementPhone", "Telefonnummer");
            var reporter = new Reporter()
            {
                Name = ReplaceChars(firstName + " " + lastName),
                Phonenumber = ReplaceChars(phoneNumber),
                Workplace = "Civilsamfund",
            };
            return reporter;
        }

        private Reporter? MapReporterInstitution(JToken? obj)
        {
            var elementInstitution = obj?
                .FirstOrDefault(z => z.Value<string>("identifier") == "ElementInstitution");
            var institutionValues = elementInstitution?.Value<JObject>("values");
            if(elementInstitution == null || institutionValues == null) return null;
            string? firstName = institutionValues.Value<string>("Fornavn");
            string? lastName = institutionValues.Value<string>("Efternavn");
            string? phoneNumber = institutionValues.Value<string>("Telefon");
            string? workplace = GetChildFromField(obj, "Workplace", "Tekst");
            string? immediateLeader = GetChildFromField(obj, "ImmediateLeader", "Tekst");
            string? immediateLeaderPhone = GetChildFromField(obj, "ImmediateLeaderPhone", "Telefonnummer");
            string? immediateLeaderEmail = GetChildFromField(obj, "ImmediateLeaderEmail", "Email");

            var reporter = new Reporter()
            {
                Name = ReplaceChars(firstName + " " + lastName),
                Phonenumber = ReplaceChars(phoneNumber),
                Workplace = ReplaceChars(workplace),
                ImmediateLeader = ReplaceChars(immediateLeader),
                ImmediateLeaderPhone = ReplaceChars(immediateLeaderPhone),
                ImmediateLeaderEmail = ReplaceChars(immediateLeaderEmail)
            };
            return reporter;
        }
    }
}
