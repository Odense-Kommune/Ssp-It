using System;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Gdf.Model.Interfaces;
using Dk.Odense.SSP.Gdf.Repository.Interface;
using Dk.Odense.SSP.Gdf.Services.Interface;
using Dk.Odense.SSP.UserCase;

namespace Dk.Odense.SSP.Gdf.Services
{
    public class AvaService<TValue> : BaseAvaService<TValue>, IGdfService<TValue> where TValue : class, IAva, new()
    {
        private readonly IGdfRepository<TValue> repository;
        private readonly IReportedPersonService reportedPersonService;
        private readonly IConcernService concernService;
        private readonly IAssessmentService assessmentService;
        private readonly IReporterService reporterService;
        private readonly IWorryService worryService;
        private readonly IUnitOfWork unitOfWork;
        private readonly HangFireUseCase hangFireUseCase;

        public AvaService(IGdfRepository<TValue> repository, IReportedPersonService reportedPersonService, IConcernService concernService, IAssessmentService assessmentService, IReporterService reporterService, IUnitOfWork unitOfWork, IWorryService worryService, HangFireUseCase hangFireUseCase) : base(repository)
        {
            this.repository = repository;
            this.reportedPersonService = reportedPersonService;
            this.concernService = concernService;
            this.assessmentService = assessmentService;
            this.reporterService = reporterService;
            this.unitOfWork = unitOfWork;
            this.worryService = worryService;
            this.hangFireUseCase = hangFireUseCase;
        }

        public async Task<bool> LoadData()
        {
            var avaList = repository.List().OrderByDescending(x => x.ID).ToList();

            foreach (var value in avaList)
            {
                if (await reportedPersonService.Get(value.ID) != null) continue;

                var reportedPerson = MapReportedPerson(value);
                var concern = MapConcern(value);
                var assessment = MapAssessment(value);
                var reporter = MapReporter(value);

                await reportedPersonService.Create(reportedPerson);
                await concernService.Create(concern);
                await assessmentService.Create(assessment);
                await reporterService.Create(reporter);

                await worryService.Create(new Worry()
                {
                    CreatedDate = DateTime.Now,
                    Concern_Id = value.ID,
                    ReportedPerson_Id = value.ID,
                    Reporter_Id = value.ID,
                    Assessment_Id = value.ID
                });

                try
                {
                    await unitOfWork.Commit();
                    Delete(value.ID);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //catch
                }
            }
            hangFireUseCase.VerifyWorries();

            return await Task.FromResult(true);
        }
    }
}
