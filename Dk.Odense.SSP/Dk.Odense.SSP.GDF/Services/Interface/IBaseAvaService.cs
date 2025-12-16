using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Gdf.Services.Interface
{
    public interface IBaseAvaService<TValue> :IBaseRobusthedService<TValue>
    {
        Concern MapConcern(TValue x);
    }
}
