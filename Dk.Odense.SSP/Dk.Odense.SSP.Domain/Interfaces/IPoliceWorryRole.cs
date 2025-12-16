using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IPoliceWorryRoleRepository : IBaseRepository<PoliceWorryRole>
    {
        Task<IEnumerable<PoliceWorryRole>> GetValidList();
    }
}
