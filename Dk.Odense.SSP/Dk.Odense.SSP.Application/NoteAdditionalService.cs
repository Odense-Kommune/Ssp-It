using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class NoteAdditionalService : BaseService<NoteAdditional, INotesAdditionalRepository>, INoteAdditionalService
    {
        public NoteAdditionalService(INotesAdditionalRepository repository) : base(repository)
        {
        }

        public async Task<IEnumerable<Note>> GetNoteByPerson(Guid personId)
        {
             return await Repository.GetAdditionalNoteByPerson(personId);
        }
    }
}