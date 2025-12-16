using System;
using System.Collections.Generic;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Agenda : Entity
    {
        public string AgendaName { get; set; }
        public int AgendaNumber { get; set; }
        public DateTime Date { get; set; }
        public bool AgendaSent { get; set; }
        public bool MeetingHeld { get; set; }

        public ICollection<AgendaItem> AgendaItems { get; set; }
    }
}
