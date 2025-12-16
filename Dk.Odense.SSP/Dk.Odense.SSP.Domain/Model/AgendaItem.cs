using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class AgendaItem : Entity
    {
        public int SortOrder { get; set; }
        public DateTime? ProcesseDate { get; set; }
        public Guid Agenda_Id { get; set; }
        public Guid? Categorization_Id { get; set; }
        [ForeignKey("Agenda_Id")]
        [JsonIgnore]
        public Agenda Agenda { get; set; }
        [ForeignKey("Categorization_Id ")]
        public Categorization Categorization { get; set; }
        public ICollection<Worry> Worries { get; set; }
    }
}
