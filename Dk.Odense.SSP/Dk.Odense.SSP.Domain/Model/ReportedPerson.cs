using Dk.Odense.SSP.Core;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dk.Odense.SSP.Domain.Model
{
    public class ReportedPerson : Entity
    {
        public string ReportedName { get; set; }
        public string ReportedCpr { get; set; }
        public string ReportedAdress { get; set; } //This is a typo and I can't change it without updating the database
        [JsonIgnore]
        public ICollection<Worry> Worries { get; set; }

    }
}
