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
    public class SourceRepository : BaseRepository<Source>, ISourceRepository
    {
        public SourceRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<IEnumerable<Source>> GetValidList()
        {
            var res = await ListQuery().Where(q => q.ValidUntil > DateTime.Now || q.ValidUntil == null).ToListAsync();

            return res;
        }
    }
}
