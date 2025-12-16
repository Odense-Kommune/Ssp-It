using System;
using System.Collections.Generic;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Categorization : Entity
    {
        public string Value { get; set; }
        public int DaysToExpire { get; set; }
        public bool DeleteAfterSspEnd { get; set; }
        public ICollection<AgendaItem> AgendaItems { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
