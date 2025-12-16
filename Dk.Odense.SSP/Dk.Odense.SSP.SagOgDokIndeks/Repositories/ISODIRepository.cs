using SagDokumentIndeks6;

namespace Dk.Odense.SSP.SagOgDokIndeks.Repositories
{
    public interface ISODIRepository
    {
        Task<IEnumerable<FiltreretOejebliksbilledeType>> SearchCaseByCpr(string cpr);
    }
}