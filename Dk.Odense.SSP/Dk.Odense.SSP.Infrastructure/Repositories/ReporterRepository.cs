using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class ReporterRepository : BaseRepository<Reporter>, IReporterRepository
    {
        public ReporterRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
