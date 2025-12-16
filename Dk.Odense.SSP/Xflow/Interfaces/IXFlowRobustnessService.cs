using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Xflow.Interfaces
{
    public interface IXFlowRobustnessService : IBaseXFlowService<XFlowRobustness>
    {
        Task<List<XFlowRobustness>?> GetNewRobustnessIds();
        bool IsValidForm(XFlowRobustness xFlowRobustness);
        Robustness? MapNewRobustness(XFlowRobustness xFlowRobustness);
        string ReplaceChars(string? str);
    }
}
