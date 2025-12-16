using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Worry : Entity
    {
        public string CrimeScene { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now.Date;
        public DateTime? Processed { get; set; }
        public DateTime? Groundless { get; set; }
        public bool Approved { get; set; } = false;
        public int Increment { get; set; }
        public Guid? Concern_Id { get; set; }
        public Guid? Person_Id { get; set; }
        public Guid? ReportedPerson_Id { get; set; }
        public Guid Reporter_Id { get; set; }
        public Guid? Assessment_Id { get; set; }
        public Guid? PoliceWorryCategory_Id { get; set; }
        public Guid? PoliceWorryRole_Id { get; set; }
        public Guid? Source_Id { get; set; }
        public Guid? AgendaItem_Id { get; set; }
        [ForeignKey("Concern_Id")]
        public Concern Concern { get; set; }
        [ForeignKey("Person_Id"), JsonIgnore]
        public Person Person { get; set; }
        [ForeignKey("ReportedPerson_Id")]
        public ReportedPerson ReportedPerson { get; set; }
        [ForeignKey("Reporter_Id")]
        public Reporter Reporter { get; set; }
        [ForeignKey("Assessment_Id")]
        public Assessment Assessment { get; set; }
        [ForeignKey("PoliceWorryCategory_Id")]
        public PoliceWorryCategory PoliceWorryCategory { get; set; }
        [ForeignKey("PoliceWorryRole_Id")]
        public PoliceWorryRole PoliceWorryRole { get; set; }
        [ForeignKey("Source_Id")]
        public Source Source { get; set; }
        [ForeignKey("AgendaItem_Id")]
        [JsonIgnore]
        public AgendaItem AgendaItem { get; set; }
        public bool PendingAutoVerify { get; set; }
    }
}
