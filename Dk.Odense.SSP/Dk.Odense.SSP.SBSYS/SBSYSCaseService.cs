using System.Threading.Tasks;
using Dk.Odense.SSP.Sbsys.Interfaces;
using Dk.Odense.SSP.Sbsys.Model;

namespace Dk.Odense.SSP.Sbsys
{
    public class SbsysCaseService : ISbsysCaseService
    {
        private readonly ISbsysCaseRepository sbsysCaseRepository;
        
        public SbsysCaseService(ISbsysCaseRepository sbsysCaseRepository)
        {
            this.sbsysCaseRepository = sbsysCaseRepository;
        }

        public async Task<SbsysCaseList> GetCases(string socialSecNum)
        {
            var result = await sbsysCaseRepository.GetCases(socialSecNum);

            return result;
        }
    }
}
