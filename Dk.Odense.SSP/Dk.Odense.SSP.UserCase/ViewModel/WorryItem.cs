using System;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class WorryItem
    {
        public Guid Id { get; set; }
        public int Increment { get; set; }
        public string Workplace { get; set; }
        public string CrimeScene { get; set; }
        public Guid? PoliceCat { get; set; }
        public Guid? PoliceRole { get; set; }
    }
}
