using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Grouping : Entity
    {
        public string Value { get; set; }

        public string Type { get; set; }

        public virtual ICollection<PersonGrouping> PersonGroupings { get; set; }
        [NotMapped] public string ClassificationName{ get; set; }
    }
}
