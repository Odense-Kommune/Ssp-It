using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application.Interfaces
{
    public interface INoteAdditionalService : IBaseService<NoteAdditional>
    {
        Task<IEnumerable<Note>> GetNoteByPerson(Guid personId);
    }
}