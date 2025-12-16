using System;
using System.Linq;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class ConcernRepository : BaseRepository<Concern>, IConcernRepository
    {
        public ConcernRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Concern GetByWorryId(Guid worryId)
        {
            return DbSet.FirstOrDefault(x => x.Id == worryId);
        }
    }
}
