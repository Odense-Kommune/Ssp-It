using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class CategorizationService : BaseService<Categorization, ICategorizationRepository>, ICategorizationService
    {
        public CategorizationService(ICategorizationRepository repository) : base(repository)
        {
        }

        public async Task<IEnumerable<Categorization>> GetValidList()
        {
            return await Repository.GetValidList();
        }
    }
}
