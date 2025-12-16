using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Xflow.Interfaces
{
    public interface IXFlowWorryService : IBaseXFlowService<XFlowWorry>
    {
        Task<List<XFlowWorry>?> GetNewWorryIds();
        bool IsValidForm(XFlowWorry worry);
        Worry? MapNewWorry(XFlowWorry xFlowWorry);
        string ReplaceChars(string? str);
    }
}
