namespace Dk.Odense.SSP.Xflow.Interfaces
{
    public interface IBaseXFlowRepository<TValue>
    {
        Task<List<TValue>?> Create(List<TValue> values);
        Task<List<TValue>> List();
        Task<bool> FindDuplicate(Guid id);
    }
}
