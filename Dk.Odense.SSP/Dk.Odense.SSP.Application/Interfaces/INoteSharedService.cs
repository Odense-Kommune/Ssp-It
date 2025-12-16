using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface INoteSharedService : IBaseService<NoteShared>
    {
        Task<IEnumerable<NoteShared>> GetNoteByPerson(Guid personId);
    }
}
