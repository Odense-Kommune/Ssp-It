using Dk.Odense.SSP.Core;
using System;
using System.Text.Json.Serialization;

namespace Dk.Odense.SSP.Domain.Model
{
    public class InternalSchoolData : Entity
    {
        [JsonIgnore]
        public Person Person { get; set; }
        public DateTime DateFirst { get; set; } = DateTime.MinValue;
        public DateTime DateLast { get; set; } = DateTime.MaxValue;
        public string Name { get; set; }
    }
}
