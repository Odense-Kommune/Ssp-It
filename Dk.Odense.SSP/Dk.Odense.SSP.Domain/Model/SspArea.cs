using System;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class SspArea : Entity
    {
        public string Value { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
