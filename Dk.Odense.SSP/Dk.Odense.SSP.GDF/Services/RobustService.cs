using System;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Gdf.Model.Interfaces;
using Dk.Odense.SSP.Gdf.Repository.Interface;
using Dk.Odense.SSP.Gdf.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Gdf.Services
{
    public class RobustService<TValue> : BaseRobusthedService<TValue>, IGdfService<TValue> where TValue : class, IRobusthed, new()
    {
        private readonly IGdfRepository<TValue> repository;
        private readonly IReportedPersonService reportedPersonService;
        private readonly IAssessmentService assessmentService;
        private readonly IReporterService reporterService;
        private readonly IRobustnessService robustnessService;
        private readonly IPersonService personService;
        private readonly IUnitOfWork unitOfWork;

        public RobustService(IGdfRepository<TValue> repository, IReportedPersonService reportedPersonService, IAssessmentService assessmentService, IReporterService reporterService, IRobustnessService robustnessService, IUnitOfWork unitOfWork, IPersonService personService) : base(repository)
        {
            this.repository = repository;
            this.reportedPersonService = reportedPersonService;
            this.assessmentService = assessmentService;
            this.reporterService = reporterService;
            this.robustnessService = robustnessService;
            this.unitOfWork = unitOfWork;
            this.personService = personService;
        }

        public async Task<bool> LoadData()
        {
            var gdfList = await List().ToListAsync();

            foreach (var value in gdfList)
            {
                try
                {
                    if (await reportedPersonService.Get(value.ID) != null) continue;

                    var reportedPerson = MapReportedPerson(value);
                    var assessment = MapAssessment(value);
                    var reporter = MapReporter(value);
                    var personId = await personService.GetBySocialSecNum(ReplaceChars(value.ReportedCpr)) != null ? (await personService.GetBySocialSecNum(ReplaceChars(value.ReportedCpr))).Id : (Guid?)null;

                    await reportedPersonService.Create(reportedPerson);
                    await assessmentService.Create(assessment);
                    await reporterService.Create(reporter);


                    if (personId != null)
                    {
                        await robustnessService.Create(new Robustness()
                        {
                            Id = value.ID,
                            ReplyRecipientName = ReplaceChars(value.ReplyRecipientName),
                            ReplyRecipientMail = ReplaceChars(value.ReplyRecipientMail),
                            CreatedDate = DateTime.Now,
                            Person_Id = (Guid)personId,
                            ReportedPerson_Id = value.ID,
                            Reporter_Id = value.ID,
                            Assessment_Id = value.ID
                        });

                        await unitOfWork.Commit();
                        Delete(value.ID);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Catch
                }
            }

            return await Task.FromResult(true);
        }
    }
}
