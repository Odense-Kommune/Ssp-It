using Dk.Odense.SSP.Domain.Model;
using System.Collections.Generic;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class ExcelUploadResult
    {
        public ReportedPerson ReportedPerson { get; set; }
        public Worry Worry { get; set; }
        public Concern Concern { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
