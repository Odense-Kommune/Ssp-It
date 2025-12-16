using System;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IRobustnessRepository : IBaseRepository<Robustness>
    {
        Task<int> GetCountByPersonId(Guid personId);
        Task<Robustness> GetNext(Guid personId, int increment = 0);
        Task<Robustness> GetPrevious(Guid personId, int increment = 0);
    }
}
