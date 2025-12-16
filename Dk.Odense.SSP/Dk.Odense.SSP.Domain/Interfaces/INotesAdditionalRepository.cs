using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Domain.Interfaces
{
    public interface INotesAdditionalRepository : IBaseRepository<NoteAdditional>
    {
        Task<IEnumerable<Note>> GetAdditionalNoteByPerson(Guid personId);
    }
}