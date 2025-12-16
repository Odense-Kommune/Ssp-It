using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Dk.Odense.SSP.Infrastructure.Repositories;

namespace Dk.Odense.SSP.Infrastructure
{
    public class AreaRuleRepository : BaseRepository<AreaRule>, IAreaRuleRepository
    {
        public AreaRuleRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
