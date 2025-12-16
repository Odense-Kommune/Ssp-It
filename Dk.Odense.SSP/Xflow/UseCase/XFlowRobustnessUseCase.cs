using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Xflow.Interfaces;

namespace Dk.Odense.SSP.Xflow.UseCase
{
    public class XFlowRobustnessUseCase
    {
        private readonly IXFlowRobustnessService _xFlowRobustnessService;
        private readonly IPersonService _personService;
        private readonly IReportedPersonService _reportedPersonService;
        private readonly IAssessmentService _assessmentService;
        private readonly IReporterService _reporterService;
        private readonly IRobustnessService _robustnessService;
        private readonly IUnitOfWork _unitOfWork;

        public XFlowRobustnessUseCase(
            IXFlowRobustnessService xFlowRobustnessService, 
            IPersonService personService,
            IReportedPersonService reportedPersonService,
            IAssessmentService assessmentService,
            IReporterService reporterService,
            IRobustnessService robustnessService,
            //HangFireUseCase hangfireUseCase,
            IUnitOfWork unitOfWork) 
        {
            _xFlowRobustnessService = xFlowRobustnessService;
            _personService = personService;
            _reportedPersonService = reportedPersonService;
            _assessmentService = assessmentService;
            _reporterService = reporterService;
            _robustnessService = robustnessService;
            //_hangfireUseCase = hangfireUseCase;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<XFlowRobustness>?> GetNewRobustnessForms()
        {
            var newIds = await _xFlowRobustnessService.GetNewRobustnessIds();
            return newIds;
        }

        public async Task<List<Robustness>?> GetNewRobustnessesXFlow()
        {
            var robustnessList = new List<Robustness>();
            var newForms = await GetNewRobustnessForms(); //Only returns the forms that have NOT already been stored in the database
            if (newForms == null || newForms.Count == 0) return null;
            foreach (var form in newForms)
            {
                //We don't make one if it already exists in the database
                var duplicateRobustness = await _robustnessService.FindDuplicate(form.Id);
                bool isValid = _xFlowRobustnessService.IsValidForm(form);
                if (duplicateRobustness || !isValid) continue;

                var robustness = _xFlowRobustnessService.MapNewRobustness(form);
                if(robustness != null)
                {
                    await CreateConnectedObjects(robustness);

                    var person = await _personService.GetBySocialSecNum(robustness.ReportedPerson.ReportedCpr);
                    Guid personId = person != null ? person.Id : Guid.Empty;

                    if (personId != Guid.Empty)
                    {
                        Robustness newRobustness = new()
                        {
                            Id = form.Id, //note: this can cause some issues if the IDs ever overlap with existing ones.
                            ReplyRecipientName = _xFlowRobustnessService.ReplaceChars(robustness.ReplyRecipientName),
                            ReplyRecipientMail = _xFlowRobustnessService.ReplaceChars(robustness.ReplyRecipientMail),
                            CreatedDate = DateTime.Now,
                            Person_Id = personId,
                            ReportedPerson_Id = robustness.ReportedPerson_Id,
                            Reporter_Id = robustness.Reporter_Id,
                            Assessment_Id = robustness.Assessment_Id,
                        };

                        try
                        {
                            //Note: we convert this to a list, because we changed the create-function to only accept lists
                            //Maybe change it back later
                            var formAsList = new List<XFlowRobustness> { form }; 

                            await _robustnessService.Create(newRobustness);
                            await _xFlowRobustnessService.Create(formAsList);
                            await _unitOfWork.Commit();

                            robustnessList.Add(newRobustness);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            throw;
                        }
                    }
                }
            }
            //I'm not sure why this runs after the robustness job, but that's how it used to work
            //_hangfireUseCase.VerifyWorries();
            return robustnessList;
        }

        async Task CreateConnectedObjects(Robustness robustness)
        {
            bool assessmentDuplicate = await _assessmentService.FindDuplicate(robustness.Assessment.Id);
            bool reportedPersonDuplicate = await _reportedPersonService.FindDuplicate(robustness.ReportedPerson.Id);
            bool reporterDuplicate = await _reporterService.FindDuplicate(robustness.Reporter.Id);

            //Only create new entry if no duplicates exist (may be unnecessary)
            if (!assessmentDuplicate) await _assessmentService.Create(robustness.Assessment);
            if (!reportedPersonDuplicate) await _reportedPersonService.Create(robustness.ReportedPerson);
            if (!reporterDuplicate) await _reporterService.Create(robustness.Reporter);
        }

    }
}
