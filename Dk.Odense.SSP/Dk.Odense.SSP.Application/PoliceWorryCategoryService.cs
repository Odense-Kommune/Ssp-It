using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class PoliceWorryCategoryService : BaseService<PoliceWorryCategory, IPoliceWorryCategoryRepository>, IPoliceWorryCategoryService
    {
        public PoliceWorryCategoryService(IPoliceWorryCategoryRepository repository) : base(repository)
        {
        }

        public Task<IEnumerable<PoliceWorryCategory>> GetValidList()
        {
            return Repository.GetValidList();
        }
    }
}
