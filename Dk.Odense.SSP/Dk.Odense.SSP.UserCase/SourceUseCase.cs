using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dk.Odense.SSP.UserCase
{
    public class SourceUseCase
    {
        ISourceService sourceService;
        public SourceUseCase(ISourceService sourceService)
        {
            this.sourceService = sourceService;
        }


        public async Task<IEnumerable<Source>> GetList()
        {
            return await sourceService.List();
        }

        public string test()
        { throw new Exception("This is Fine!"); }
    }
}
