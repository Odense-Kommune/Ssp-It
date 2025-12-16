using System;
using Newtonsoft.Json;

namespace Dk.Odense.SSP.ExternalServices.Model
{
    public class SchoolData
    {
        public DateTime DateFirst { get; set; }
        public DateTime DateLast { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string Type { get; set; }
     }
}
