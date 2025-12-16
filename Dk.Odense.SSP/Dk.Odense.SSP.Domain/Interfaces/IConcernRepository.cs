using System;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface IConcernRepository : IBaseRepository<Concern>
    {
        Concern GetByWorryId(Guid worryId);
    }
}
