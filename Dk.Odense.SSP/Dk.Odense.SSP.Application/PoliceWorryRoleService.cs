using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class PoliceWorryRoleService : BaseService<PoliceWorryRole, IPoliceWorryRoleRepository>, IPoliceWorryRoleService
    {
        public PoliceWorryRoleService(IPoliceWorryRoleRepository repository) : base(repository)
        {
        }

        public async Task<IEnumerable<PoliceWorryRole>> GetValidList()
        {
            return await Repository.GetValidList();
        }
    }
}
