using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Person : Entity
    {
        public string SocialSecNum { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }
        public DateTime? SspStopDate { get; set; }
        public string SocialWorker { get; set; }
        public Guid? SspArea_Id { get; set; }
        [ForeignKey("SspArea_Id ")]
        public SspArea SspArea { get; set; }
        public ICollection<PersonGrouping> PersonGroupings { get; set; }
        [JsonIgnore]
        public ICollection<Worry> Worries { get; set; }
        public ICollection<Robustness> Robustnesses { get; set; }
        public ICollection<Note> Notes { get; set; }
        public DateTime LastVerified { get; set; }
        [ForeignKey("SchoolData_Id")]
        public InternalSchoolData SchoolData { get; set; }
        public Guid? SchoolData_Id { get; set; }
    }
}
