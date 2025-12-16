using System;
using System.Collections.Generic;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class ExportCrossRefDto
    {
        public Guid Id { get; set; }
        public string GroupingsType { get; set; }
        public IEnumerable<Guid> SelectedPersons { get; set; }
        public IEnumerable<Guid> SelectedCategorizations { get; set; }

    }
}
