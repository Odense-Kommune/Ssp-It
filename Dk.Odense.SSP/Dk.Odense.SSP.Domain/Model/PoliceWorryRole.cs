using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class PoliceWorryRole : Entity
    {
        public PoliceWorryRole()
        {
            Worries = new HashSet<Worry>();
        }

        public PoliceWorryRole(Guid id)
        {
            Id = id;
        }

        public string Value { get; set; }
        [JsonIgnore]
        public ICollection<Worry> Worries { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
