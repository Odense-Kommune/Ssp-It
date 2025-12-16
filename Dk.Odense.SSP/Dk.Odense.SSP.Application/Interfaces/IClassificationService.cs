using Dk.Odense.SSP.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface IClassificationService: IBaseService<Classification>
    {
        Task<IEnumerable<Classification>> GetValidList();
    }
}