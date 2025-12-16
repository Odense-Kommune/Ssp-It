using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class SourceService : BaseService<Source, ISourceRepository>, ISourceService
    {
        public SourceService(ISourceRepository repository) : base(repository)
        {

        }

        public Task<IEnumerable<Source>> GetValidList()
        {
            return Repository.GetValidList();
        }
    }
}
