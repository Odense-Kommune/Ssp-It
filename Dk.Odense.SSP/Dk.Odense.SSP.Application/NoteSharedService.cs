using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Application
{
    public class NoteSharedService : BaseService<NoteShared, INoteSharedRepository>, INoteSharedService
    {
        public NoteSharedService(INoteSharedRepository repository) : base(repository)
        {

        }

        public async Task<IEnumerable<NoteShared>> GetNoteByPerson(Guid personId)
        {
            return await Repository.GetNoteByPerson(personId);
        }
    }
}
