using System.Linq;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IBaseQueryRepository<TValue> : IBaseRepository<TValue>
    {
        IQueryable<TValue> ListQuery();
    }
}
