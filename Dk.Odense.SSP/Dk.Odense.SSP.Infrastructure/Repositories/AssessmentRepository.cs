using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class AssessmentRepository : BaseRepository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
