using Dk.Odense.SSP.Core;
using System.Text.Json.Serialization;
using Enum = Dk.Odense.SSP.Core.Enum;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Concern : Entity
    {
        public string CrimeConcern { get; set; }
        public Enum.Answer ReportedToPolice { get; set; }
        public Enum.Answer NotifyConcern { get; set; }
        [JsonIgnore]
        public Worry Worry { get; set; }
    }
}
