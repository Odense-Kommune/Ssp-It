using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Interfaces;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dk.Odense.SSP.Infrastructure.Repositories
{
    public class NoteAdditionalRepository : BaseRepository<NoteAdditional>, INotesAdditionalRepository
    {
        public NoteAdditionalRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<IEnumerable<Note>> GetAdditionalNoteByPerson(Guid personId)
        {
            var res = await ListQuery().Where(x => x.Person_Id == personId).ToListAsync();

            return res;
        }
    }
}