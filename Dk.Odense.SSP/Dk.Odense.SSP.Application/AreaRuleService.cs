using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class AreaRuleService : BaseService<AreaRule, IAreaRuleRepository>, IAreaRuleService
    {
        public AreaRuleService(IAreaRuleRepository repository) : base(repository)
        {
        }
    }
}
