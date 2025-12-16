using System;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class ConcernService : BaseService<Concern, IConcernRepository>, IConcernService
    {
        public ConcernService(IConcernRepository repository) : base(repository)
        {
        }

        public Concern GetByWorryId(Guid worryId)
        {
            return Repository.GetByWorryId(worryId);
        }
    }
}
