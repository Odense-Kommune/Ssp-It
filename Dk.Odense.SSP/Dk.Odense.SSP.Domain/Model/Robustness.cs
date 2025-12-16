using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Robustness : Entity
    {
        public string ReplyRecipientName { get; set; }
        public string ReplyRecipientMail { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Increment { get; set; }
        public string EnrollmentPlace { get; set; }
        //ForeignKey Guids
        public Guid Person_Id { get; set; }
        public Guid ReportedPerson_Id { get; set; }
        public Guid Reporter_Id { get; set; }
        public Guid Assessment_Id { get; set; }
        //Foreign Models
        [ForeignKey("Person_Id"), JsonIgnore]
        public Person Person { get; set; }
        [ForeignKey("ReportedPerson_Id")]
        public ReportedPerson ReportedPerson { get; set; }
        [ForeignKey("Reporter_Id")]
        public Reporter Reporter { get; set; }
        [ForeignKey("Assessment_Id")]
        public Assessment Assessment { get; set; }
    }
}
