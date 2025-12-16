using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IBaseRepository<TValue>
    {
        Task<TValue> Get(Guid id);
        Task<IEnumerable<TValue>> List();
        Task<TValue> Create(TValue value);
        TValue Update(TValue value);
        Task<bool> Delete(Guid id);
        Task<bool> FindDuplicate(Guid id);
    }
}
