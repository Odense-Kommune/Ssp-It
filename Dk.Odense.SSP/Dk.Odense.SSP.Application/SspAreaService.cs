using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class SspAreaService : BaseService<SspArea, ISspAreaRepository>, ISspAreaService
    {
        public SspAreaService(ISspAreaRepository repository) : base(repository)
        {
        }

        public Task<IEnumerable<SspArea>> GetValidList()
        {
            return Repository.GetValidList();
        }
    }
}
