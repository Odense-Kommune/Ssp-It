using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Dk.Odense.SSP.Xflow.Interfaces;

namespace Dk.Odense.SSP.Xflow.Repositories
{
    public class XFlowRobustnessRepository : BaseXFlowRepository<XFlowRobustness>, IXFlowRobustnessRepository
    {
        public XFlowRobustnessRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
