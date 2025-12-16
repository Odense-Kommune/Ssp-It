using System;
using System.Threading;
using System.Threading.Tasks;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Infrastructure.Interfaces;

namespace Dk.Odense.SSP.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDatabaseContext databaseContext;
        private bool disposed = false;

        public UnitOfWork(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
            //databaseContext.Database.Log = s => Debug.WriteLine(s);
        }

        public async Task Commit()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            try
            {
                await databaseContext.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                databaseContext?.Dispose();
            }

            disposed = true;
        }
    }
}
