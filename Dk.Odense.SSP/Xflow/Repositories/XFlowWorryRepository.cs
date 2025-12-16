using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Dk.Odense.SSP.Xflow.Interfaces;

namespace Dk.Odense.SSP.Xflow.Repositories
{
    public class XFlowWorryRepository : BaseXFlowRepository<XFlowWorry>, IXFlowWorryRepository
    {
        public XFlowWorryRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {

        }
    }
}
