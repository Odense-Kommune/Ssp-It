using System;
using System.Collections.Generic;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Source : Entity
    {
        public string Value { get; set; }
        public ICollection<Worry> Worry { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
