using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.Gdf.Services.Interface
{
    public interface IGdfService<TValue>
    {
        IQueryable<TValue> List();
        Task<bool> LoadData();
        bool Delete(Guid id);
    }
}
