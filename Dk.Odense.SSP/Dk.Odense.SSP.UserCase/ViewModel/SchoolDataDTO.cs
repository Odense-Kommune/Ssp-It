using System;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class SchoolDataDTO
    {
        public DateTime DateFirst { get; set; } = DateTime.MinValue;
        public DateTime DateLast { get; set; } = DateTime.MaxValue;
        public string Name { get; set; }
    }
}
