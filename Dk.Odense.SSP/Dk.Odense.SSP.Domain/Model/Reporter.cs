using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Reporter : Entity
    {
        public string Name { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string Workplace { get; set; }
        public string ImmediateLeader { get; set; }
        public string ImmediateLeaderEmail { get; set; }
        public string ImmediateLeaderPhone { get; set; }
    }
}
