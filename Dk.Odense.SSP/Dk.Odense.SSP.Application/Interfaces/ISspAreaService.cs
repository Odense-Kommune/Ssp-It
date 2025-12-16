using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface ISspAreaService : IBaseService<SspArea>
    {
        Task<IEnumerable<SspArea>> GetValidList();
    }
}
