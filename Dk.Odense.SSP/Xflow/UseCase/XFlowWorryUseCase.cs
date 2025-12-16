using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Xflow.Interfaces;
using Microsoft.Extensions.Options;

namespace Dk.Odense.SSP.Xflow.UseCase
{
    public class XFlowWorryUseCase
    {
        private readonly IXFlowWorryService _xFlowWorryService;
        private readonly IAssessmentService _assessmentService;
        private readonly IConcernService _concernService;
        private readonly IReportedPersonService _reportedPersonService;
        private readonly IReporterService _reporterService;
        private readonly IWorryService _worryService;
        private readonly XFlowConfig _xFlowConfig;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Guid _xFlowId;

        public XFlowWorryUseCase(
            IXFlowWorryService xFlowWorryService,
            IAssessmentService assessmentService,
            IConcernService concernService,
            IReportedPersonService reportedPersonService,
            IReporterService reporterService,
            IWorryService worryService,
            IOptions<XFlowConfig> xFlowConfig,
            IUnitOfWork unitOfWork)
        {
            _xFlowWorryService = xFlowWorryService;
            _assessmentService = assessmentService;
            _concernService = concernService;
            _reportedPersonService = reportedPersonService;
            _reporterService = reporterService;
            _worryService = worryService;
            _xFlowConfig = xFlowConfig.Value;
            _xFlowId = Guid.Parse(_xFlowConfig.WorrySourceId);
            _unitOfWork = unitOfWork;
        }

        public async Task<List<XFlowWorry>?> GetNewWorryForms()
        {
            var newIds = await _xFlowWorryService.GetNewWorryIds();
            return newIds;
        }

        public async Task<IEnumerable<Worry>?> GetNewWorriesXFlow()
        {
            var worryList = new List<Worry>();
            var newForms = await GetNewWorryForms();
            if (newForms == null || newForms.Count == 0) return null;
            foreach (var form in newForms)
            {
                //we don't want to make one if it already exists in the database
                var duplicateWorry = await _worryService.FindDuplicate(form.Id);
                bool isValid = _xFlowWorryService.IsValidForm(form);
                if (duplicateWorry || !isValid) continue;

                var worry = _xFlowWorryService.MapNewWorry(form);
                if (worry != null)
                {
                    await CreateConnectedObjects(worry);

                    Worry newWorry = new()
                    {
                        Id = form.Id, //note: this can cause some issues if the IDs ever overlap with existing ones.
                        CreatedDate = DateTime.Now,
                        Concern_Id = worry.Concern_Id,
                        ReportedPerson_Id = worry.ReportedPerson_Id,
                        Reporter_Id = worry.Reporter_Id,
                        Assessment_Id = worry.Assessment_Id,
                        Source_Id = _xFlowId
                    };

                    try
                    {
                        //Same as in robustness. Maybe change back later.
                        var formAsList = new List<XFlowWorry> { form };

                        await _worryService.Create(newWorry);
                        await _xFlowWorryService.Create(formAsList);
                        await _unitOfWork.Commit();

                        worryList.Add(newWorry);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }
            return worryList;
        }

        async Task CreateConnectedObjects(Worry worry)
        {
            bool assessmentDuplicate = await _assessmentService.FindDuplicate(worry.Assessment.Id);
            bool reportedPersonDuplicate = await _reportedPersonService.FindDuplicate(worry.ReportedPerson.Id);
            bool reporterDuplicate = await _reporterService.FindDuplicate(worry.Reporter.Id);
            bool concernDuplicate = await _concernService.FindDuplicate(worry.Concern.Id);

            //Only create new entry if no duplicates exist (may be unnecessary)
            if (!assessmentDuplicate) await _assessmentService.Create(worry.Assessment);
            if (!reportedPersonDuplicate) await _reportedPersonService.Create(worry.ReportedPerson);
            if (!reporterDuplicate) await _reporterService.Create(worry.Reporter);
            if (!concernDuplicate) await _concernService.Create(worry.Concern);
        }
    }
}
