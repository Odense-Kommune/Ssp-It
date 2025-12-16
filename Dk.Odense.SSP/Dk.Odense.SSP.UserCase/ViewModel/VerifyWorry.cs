using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class VerifyWorry
    {
        public string SocialSecNum { get; set; }
        public Reporter Reporter { get; set; }
        public ReportedPerson ReportedPerson { get; set; }
        public Concern Concern { get; set; }
        public Assessment Assessment { get; set; }
        public PoliceWorryCategory PoliceWorryCategory { get; set; }
        public PoliceWorryRole PoliceWorryRole { get; set; }
    }
}
