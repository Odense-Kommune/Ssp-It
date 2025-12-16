using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface ISourceRepository : IBaseRepository<Source>
    {
        Task<IEnumerable<Source>> GetValidList();
    }
}
