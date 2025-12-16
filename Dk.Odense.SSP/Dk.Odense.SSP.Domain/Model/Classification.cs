using Dk.Odense.SSP.Core;
using System;
using System.Collections.Generic;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Classification : Entity
    {
        public string Value { get; set; }
        public DateTime? ValidUntil { get; set; }
        public virtual ICollection<PersonGrouping> PersonGroupings { get; set; }
    }
}