using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class ClassificationService : BaseService<Classification, IClassificationRepository>, IClassificationService
    {
        public ClassificationService(IClassificationRepository repository) : base(repository)
        {
        }

        public async Task<IEnumerable<Classification>> GetValidList()
        {
            return await Repository.GetValidList();
        }
    }
}