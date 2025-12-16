using System;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class RobustnessService : BaseService<Robustness, IRobustnessRepository>, IRobustnessService
    {
        private readonly IRobustnessRepository robustnessRepository;

        public RobustnessService(IRobustnessRepository repository) : base(repository)
        {
            this.robustnessRepository = repository;
        }

        public async Task<int> GetCountByPersonId(Guid personId)
        {
            return await robustnessRepository.GetCountByPersonId(personId);
        }

        public Task<Robustness> GetNext(Guid personId, int increment = 0)
        {
            var res = Repository.GetNext(personId, increment);

            return res;
        }

        public async Task<Robustness> GetPrevious(Guid personId, int increment = 0)
        {
            var res = await Repository.GetPrevious(personId, increment);

            return res;
        }
    }
}
