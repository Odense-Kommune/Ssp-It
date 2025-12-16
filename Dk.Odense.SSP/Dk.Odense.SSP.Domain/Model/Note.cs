using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Note : Entity
    {
        public string Value { get; set; }
        public string Reporter { get; set; }
        public Guid Person_Id { get; set; }
        [ForeignKey("Person_Id"), JsonIgnore]
        public Person Person { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Discriminator { get; set; }
    }
}
