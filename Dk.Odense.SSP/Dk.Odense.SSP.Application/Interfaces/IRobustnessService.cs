using System;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IRobustnessService : IBaseService<Robustness>
    {
        Task<int> GetCountByPersonId(Guid personId);
        Task<Robustness> GetNext(Guid personId, int increment = 0);
        Task<Robustness> GetPrevious(Guid personId, int increment = 0);
    }
}
