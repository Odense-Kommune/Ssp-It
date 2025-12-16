using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface ISspAreaRepository : IBaseRepository<SspArea>
    {
        Task<IEnumerable<SspArea>> GetValidList();
    }
}
