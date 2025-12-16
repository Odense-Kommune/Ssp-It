using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class SchoolDataRepository : BaseRepository<InternalSchoolData>, ISchoolDataRepository
    {
        public SchoolDataRepository(IDatabaseContext databaseContext) : base(databaseContext) { }
    }
}
