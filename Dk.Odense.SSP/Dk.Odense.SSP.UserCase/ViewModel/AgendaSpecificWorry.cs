using System.Collections.Generic;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class AgendaSpecificWorry : Entity
    {
        public Worry Worry { get; set; }
        public IEnumerable<Robustness> Robustnesses { get; set; }
        public  IEnumerable<Note> Notes { get; set; }
    }
}
