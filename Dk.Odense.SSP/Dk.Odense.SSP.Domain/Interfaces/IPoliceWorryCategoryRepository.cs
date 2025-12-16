using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IPoliceWorryCategoryRepository : IBaseRepository<PoliceWorryCategory>
    {
        Task<IEnumerable<PoliceWorryCategory>> GetValidList();
    }
}
