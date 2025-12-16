using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Xflow.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Dk.Odense.SSP.Xflow.Services
{
    public class XFlowRobustnessService
        : BaseXFlowService<XFlowRobustness, IXFlowRobustnessRepository>,
            IXFlowRobustnessService
    {
        private readonly IXFlowRobustnessRepository _repository;
        private readonly XFlowConfig _xFlowConfig;
        private readonly HttpClient _httpClient;

        private readonly string[] publicId;
        private readonly string reportedPersonFormName;
        private readonly string assessmentFormName;
        private readonly string reporterFormName;

        public XFlowRobustnessService(
            IXFlowRobustnessRepository repository,
            IOptions<XFlowConfig> xFlowConfig
        )
            : base(repository, xFlowConfig)
        {
            _repository = repository;
            _xFlowConfig = xFlowConfig.Value;
            _httpClient = CreateClient();

            // Set form variables
            publicId = [_xFlowConfig.RobustnessSchemaId];
            reportedPersonFormName = _xFlowConfig.RobustnessReportedPersonFormName;
            assessmentFormName = _xFlowConfig.RobustnessAssessmentFormName;
            reporterFormName = _xFlowConfig.RobustnessReporterFormName;
        }

        public async Task<List<XFlowRobustness>?> GetNewRobustnessIds()
        {
            IEnumerable<JToken>? robustnessIds = await GetFormIds(publicId);
            if (robustnessIds != null && robustnessIds.ToList().Count > 0)
            {
                var idList = new List<XFlowRobustness>();
                foreach (var robustnessId in robustnessIds)
                {
                    var publicId = robustnessId.Value<string>("publicId");
                    if (publicId == null)
                        continue;
                    var guid = Guid.Parse(publicId);
                    var result = await GetFormsFromXFlow(guid);
                    var xFlowRobustness = new XFlowRobustness()
                    {
                        Id = guid,
                        Value = result?.ToString(),
                    };
                    bool alreadyExists = await FindDuplicate(guid);
                    if (!alreadyExists)
                    {
                        idList.Add(xFlowRobustness);
                    }
                }
                return idList;
            }
            return null;
        }

        //Can be moved to BaseService
        public bool IsValidForm(XFlowRobustness xFlowRobustness)
        {
            if (xFlowRobustness != null)
            {
                var formData = JToken.Parse(xFlowRobustness.Value);
                if (formData != null)
                {
                    var status = formData.Value<string>("status");
                    return status == "Afsluttet";
                }
            }
            return false;
        }

        public Robustness? MapNewRobustness(XFlowRobustness xFlowRobustness)
        {
            if (xFlowRobustness != null)
            {
                var robustness = new Robustness();
                var formData = JToken.Parse(xFlowRobustness.Value);
                if (formData != null)
                    robustness = MapFormGroup(formData, robustness);
                if (robustness != null)
                    return robustness;
            }
            return null;
        }

        private Robustness? MapFormGroup(JToken formGroup, Robustness robustness)
        {
            var forms = formGroup.Value<JArray>("blanketter");
            if (forms == null)
                return null;
            foreach (var form in forms)
            {
                ReportedPerson? reportedPerson = null;
                Assessment? assessment = null;
                Reporter? reporter = null;

                var formName = form.Value<string>("blanketnavn");

                if (formName == null)
                    continue; // This should not happen, but just in case

                switch (formName)
                {
                    case var name when name.Equals(reportedPersonFormName):
                        reportedPerson = MapReportedPerson(form);
                        break;

                    case var name when name.Equals(assessmentFormName):
                        assessment = MapAssessment(form);
                        break;

                    case var name when name.Equals(reporterFormName):
                        reporter = MapReporter(form);
                        robustness = MapRobustnessReplyRecipient(form, robustness);
                        break;
                }

                // Assign mapped objects to robustness if they are not null
                if (reportedPerson != null)
                {
                    robustness.ReportedPerson_Id = reportedPerson.Id;
                    robustness.ReportedPerson = reportedPerson;
                }
                if (assessment != null)
                {
                    robustness.Assessment_Id = assessment.Id;
                    robustness.Assessment = assessment;
                }
                if (reporter != null)
                {
                    robustness.Reporter_Id = reporter.Id;
                    robustness.Reporter = reporter;
                }
            }
            return robustness;
        }

        private ReportedPerson? MapReportedPerson(JToken form)
        {
            var elements = form.Value<JArray>("elementer");
            var elementPerson = elements?.FirstOrDefault(e =>
                e.Value<string>("identifier") == "ElementPerson"
            );
            var values = elementPerson?.Value<JObject>("values");
            if (elementPerson != null && values != null)
            {
                string? firstName = GetChildFromField(elements, "ElementPerson", "Fornavn");
                string? lastName = GetChildFromField(elements, "ElementPerson", "Efternavn");
                string? socialSecurityNumber = GetChildFromField(
                    elements,
                    "ElementPerson",
                    "CprNummer"
                );
                var reportedPerson = new ReportedPerson()
                {
                    ReportedName = ReplaceChars(firstName + " " + lastName),
                    ReportedCpr = ReplaceChars(FormatSocialSecNum(socialSecurityNumber)),
                };
                return reportedPerson;
            }
            return null;
        }

        private Reporter? MapReporter(JToken form)
        {
            var elements = form.Value<JArray>("elementer");
            var elementPerson = elements?.FirstOrDefault(e =>
                e.Value<string>("identifier") == "ElementPerson"
            );
            var values = elementPerson?.Value<JObject>("values");
            if (elementPerson != null && values != null)
            {
                string? firstName = values.Value<string>("Fornavn");
                string? lastName = values.Value<string>("Efternavn");
                string? email = values.Value<string>("Email");
                string? workplace = values.Value<string>("Afdeling");
                string? immediateLeader = GetChildFromField(elements, "ImmediateLeader", "Tekst");
                string? immediateLeaderPhone = GetChildFromField(
                    elements,
                    "ImmediateLeaderPhone",
                    "Tekst"
                );
                string? immediateLeaderEmail = GetChildFromField(
                    elements,
                    "ImmediateLeaderEmail",
                    "Tekst"
                );

                var reporter = new Reporter()
                {
                    Name = ReplaceChars(firstName + " " + lastName),
                    Email = ReplaceChars(email),
                    Workplace = ReplaceChars(workplace),
                    ImmediateLeader = ReplaceChars(immediateLeader),
                    ImmediateLeaderPhone = ReplaceChars(immediateLeaderPhone),
                    ImmediateLeaderEmail = ReplaceChars(immediateLeaderEmail),
                };

                return reporter;
            }
            return null;
        }

        private Robustness MapRobustnessReplyRecipient(JToken form, Robustness robustness)
        {
            var obj = form.Value<JArray>("elementer");
            var replyRecipientName = GetChildFromField(obj, "ReplyRecipientName", "Tekst");
            var replyRecipientMail = GetChildFromField(obj, "ReplyRecipientMail", "Email");
            robustness.ReplyRecipientName = ReplaceChars(replyRecipientName);
            robustness.ReplyRecipientMail = ReplaceChars(replyRecipientMail);
            return robustness;
        }
    }
}
