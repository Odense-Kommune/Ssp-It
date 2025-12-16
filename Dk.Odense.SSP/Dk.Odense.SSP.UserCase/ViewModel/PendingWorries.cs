using Dk.Odense.SSP.Domain.Model;
using System;
using System.Collections.Generic;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class PendingWorry
    {
        public Domain.Model.Person Person { get; set; }
        public List<Worry> Worries { get; set; }    
        public Boolean Checked { get; set; } = true;
        public Guid AgendaId { get; set; }
        public PendingWorry()
        {
            Worries = new List<Worry>();
        }
    }
}
