using System;
using System.Linq;

namespace Dk.Odense.SSP.Gdf.Repository.Interface
{
    public interface IGdfRepository<TValue>
    {
        IQueryable<TValue> List();

        bool Delete(Guid id);
    }
}
