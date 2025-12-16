using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface INoteSharedRepository : IBaseRepository<NoteShared>
    {
        Task<IEnumerable<NoteShared>> GetNoteByPerson(Guid personId);
    }
}
