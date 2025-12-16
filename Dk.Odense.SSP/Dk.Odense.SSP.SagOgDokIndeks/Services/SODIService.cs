
using Dk.Odense.SSP.SagOgDokIndeks.Repositories;
using SagDokumentIndeks6;
using System.Xml;

namespace Dk.Odense.SSP.SagOgDokIndeks.Services
{
    public class SODIService : ISODIService
    {
        private readonly ISODIRepository sODIRepository;

        public SODIService(ISODIRepository sODIRepository)
        {
            this.sODIRepository = sODIRepository;
        }

        public async Task<IEnumerable<SODISimple>> SearchCaseByCpr(string cpr)
        {
            var res = await sODIRepository.SearchCaseByCpr(cpr);

            var cases = new List<SODISimple>();

            foreach (var item in res.Select(x => x.Registrering))
            {
                cases.Add(await MapToSODISimple(item, cpr));
            }

            return cases.Where(x => x.Status != "Afsluttet").ToList();
        }

        private async Task<SODISimple> MapToSODISimple(RegistreringType2[] item, string cpr)
        {
            return new SODISimple()
            {
                Afdeling = GetDepartment(item),
                Kle = GetKleNumber(item),
                ItSystem = GetSystemName(item),
                Status = GetStatus(item)
            };
        }
        private static string GetStatus(RegistreringType2[] items)
        {
            // Directly find the first status code across all items.
            var statusCode = items.SelectMany(x => x.TilstandListe.Fremdrift)
                                  .FirstOrDefault()?.FremdriftStatusKode;

            return statusCode?.ToString() ?? "";
        }

        private static string GetDepartment(RegistreringType2[] items)
        {
            // Directly find the first department matching the specified type across all items.
            var departmentId = items.SelectMany(x => x.RelationListe.Sagsaktoer)
                                    .FirstOrDefault(x => x.Type.Item == "c5fc3b3b-5197-49ee-92e6-ae6ba1957174")?.ReferenceID.Item;

            // If a department ID is found and it's a valid GUID, return it; otherwise, return an empty string.
            return Guid.TryParse(departmentId, out Guid _) ? departmentId : "";
        }

        private static string GetKleNumber(RegistreringType2[] items)
        {
            // Directly find the first matching KLE number without collecting all matches in a list first.
            var kle = items.Where(x => x.RelationListe != null)
                           .SelectMany(x => x.RelationListe?.Sagsklasse ?? new RelationType[0])
                           .FirstOrDefault(x => x?.Type.Item == "267235ea-526d-4a18-8001-f2a0e563eba1")?.ReferenceID.Item;
            return kle ?? "";
        }

        private static string GetSystemName(RegistreringType2[] items)
        {
            // Assuming the first item's RelationListe's LokalUdvidelseListe is representative for all items.
            var systemRelationNode = items
                .SelectMany(x => x.RelationListe.LokalUdvidelseListe.Any)
                .FirstOrDefault()?.ChildNodes;

            if (systemRelationNode == null) return "";

            // Find the first node that contains "SystemNavn" in its name and get its inner text.
            var systemName = systemRelationNode.Cast<XmlNode>()
                .FirstOrDefault(node => node.Name.Contains("SystemNavn"))?.InnerText ?? "";

            // Simplify names for "Nexus" and "Momentum"
            if (systemName.Contains("Nexus")) return "Nexus";
            if (systemName.Contains("Momentum")) return "Momentum";

            return systemName; 
        }
    }
}