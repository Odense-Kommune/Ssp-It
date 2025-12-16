using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface ISourceService : IBaseService<Source>
    {
        Task<IEnumerable<Source>> GetValidList();
    }
}
