using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class AssessmentService : BaseService<Assessment, IAssessmentRepository>, IAssessmentService
    {
        public AssessmentService(IAssessmentRepository repository) : base(repository)
        {
        }
    }
}
