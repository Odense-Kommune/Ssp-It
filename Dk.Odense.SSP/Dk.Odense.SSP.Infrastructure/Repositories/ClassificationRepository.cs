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
    public class ClassificationRepository : BaseRepository<Classification>, IClassificationRepository
    {
        public ClassificationRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<IEnumerable<Classification>> GetValidList()
        {
            return await ListQuery().Where(x => x.ValidUntil > DateTime.Now || x.ValidUntil == null).ToListAsync();
        }
    }
}