using System.IO;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class ExportData
    {
        public string RootFolder { get; set; }
        public string FileName { get; set; }
        public FileInfo FileInfo { get; set; }
    }
}
