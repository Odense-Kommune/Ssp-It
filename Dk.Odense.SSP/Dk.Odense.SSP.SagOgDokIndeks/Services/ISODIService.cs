namespace Dk.Odense.SSP.SagOgDokIndeks.Services
{
    public interface ISODIService
    {
        Task<IEnumerable<SODISimple>> SearchCaseByCpr(string cpr);
    }
}