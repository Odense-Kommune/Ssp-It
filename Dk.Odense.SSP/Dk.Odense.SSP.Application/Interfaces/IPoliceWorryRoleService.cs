using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IPoliceWorryRoleService : IBaseService<PoliceWorryRole>
    {
        Task<IEnumerable<PoliceWorryRole>> GetValidList();
    }
}
