using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface ICategorizationService : IBaseService<Categorization>
    {
        Task<IEnumerable<Categorization>> GetValidList();
    }
}
