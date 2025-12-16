using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class PersonGrouping : Entity
    {
        public Guid Person_Id { get; set; }
        public Guid Grouping_Id { get; set; }
        public Guid? Classification_Id { get; set; }
        [ForeignKey("Person_Id"), JsonIgnore]
        public Person Person { get; set; }
        [ForeignKey("Grouping_Id"), JsonIgnore]
        public Grouping Grouping { get; set; }
        [ForeignKey("Classification_Id")]
        public Classification Classification { get; set; }

    }
}
