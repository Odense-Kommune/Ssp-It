using Newtonsoft.Json.Linq;

namespace Dk.Odense.SSP.Xflow.Interfaces
{
    public interface IBaseXFlowService<TValue>
    {
        Task<List<TValue>?> Create(List<TValue> values);
        Task<List<TValue>> List();
        Task<IEnumerable<JToken>?> GetFormIds(string[] publicId);
        Task<bool> FindDuplicate(Guid id);
    }
}
