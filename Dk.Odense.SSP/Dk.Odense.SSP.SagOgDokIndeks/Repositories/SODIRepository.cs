using Dk.Odense.SSP.SagOgDokIndeks.Helpers;
using SagDokumentIndeks6;

namespace Dk.Odense.SSP.SagOgDokIndeks.Repositories
{
    public class SODIRepository : ISODIRepository
    {
        private readonly string service = "sagdokumentindeks/6";

        public async Task<IEnumerable<FiltreretOejebliksbilledeType>> SearchCaseByCpr(string cpr)
        {
            try
            {
                var req = new fremsoegRequest()
                {
                    FremsoegSagDokumentIndeksInput = new FremsoegSagDokumentIndeksInputType()
                    {
                        SoegUdtryk = new SoegUdtrykType()
                        {
                            Items =
                            [ new SoegInputType1()
                            { 
                                TilstandListe = new TilstandListeType(),
                                AttributListe = new AttributListeType(),
                                RelationListe = new RelationListeType()
                                {
                                    Sagspart = [
                                    new RelationType() {
                                        ReferenceID = new UnikIdType(){
                                            Item = "urn:oio:cpr-nr:"+cpr, ItemElementName = ItemChoiceType.URNIdentifikator}
                                }]
                            }
                        }
                            ],
                            ItemsElementName = [ItemsChoiceType.SoegSag]
                        },
                        Filter = new object[] {
                            new SagVisType()
                            {
                               Vis = SagVisFilterType.itsystem,
                            },
                            new SagVisType()
                            {
                               Vis = SagVisFilterType.sagspart,
                            },
                            new SagVisType()
                            {
                               Vis = SagVisFilterType.fremdrift,
                            },
                            new SagVisType()
                            {
                               Vis = SagVisFilterType.sagsaktoer,
                            },
                            new SagVisType()
                            {
                               Vis = SagVisFilterType.sagsklasse,
                            },
                    }
                    }
                };


                var channel = ConnectionUtility.CreateChannel<SagDokumentIndeksPortType>(service, "fremsoeg");

                var res = await channel.fremsoegAsync(req);
                if(res.FremsoegSagDokumentIndeksOutput.SagFiltreretOejebliksbillede == null) return [];
                return (from item in res.FremsoegSagDokumentIndeksOutput.SagFiltreretOejebliksbillede
                        select item).ToList();
            }
            catch (Exception e)
            {

                throw;
            }
        }

    }
}