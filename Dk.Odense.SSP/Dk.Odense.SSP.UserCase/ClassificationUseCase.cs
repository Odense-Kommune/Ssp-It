using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.UserCase
{
    public class ClassificationUseCase
    {
        private readonly IClassificationService classificationService;

        public ClassificationUseCase(IClassificationService classificationService)
        {
            this.classificationService = classificationService;
        }

        public async Task<IEnumerable<Classification>> GetList()
        {
            return await classificationService.GetValidList();
        }
    }
}