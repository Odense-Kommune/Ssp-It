using System;
using System.Collections.Generic;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class AgendaSpecificMenuItem
    {
        public Guid Id { get; set; }
        public PersonDto Person { get; set; }
        public IEnumerable<WorryItem> WorryItems { get; set; }
        public int SortOrder { get; set; }
    }
}
