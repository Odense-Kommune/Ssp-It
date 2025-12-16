using Dk.Odense.SSP.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IClassificationRepository : IBaseRepository<Classification>
    {
        Task<IEnumerable<Classification>> GetValidList();
    }
}