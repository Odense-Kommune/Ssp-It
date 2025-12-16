using System;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class VerifyWorryMenuItem
    {
        public Guid Id { get; set; }
        public int Increment { get; set; }
        public string Source { get; set; }
        public ReportedPerson ReportedPerson { get; set; }
        public bool Groundless { get; set; }
        public bool Approved { get; set; }
        public NavneOpslagData SocialSecData { get; set; }
    }
}
