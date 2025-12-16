using System;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class ReporterService : BaseService<Reporter, IReporterRepository>,IReporterService 
    {
        public ReporterService(IReporterRepository repository) : base(repository)
        {
        }

        public Reporter GetByWorryId(Guid worryId)
        {
            throw new NotImplementedException();
        }
    }
}
