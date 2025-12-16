using System.Threading.Tasks;

namespace Dk.Odense.SSP.Core
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}