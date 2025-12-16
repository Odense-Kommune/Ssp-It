using Dk.Odense.SSP.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dk.Odense.SSP.Domain.Model
{
    public class AreaRule : Entity
    {
        public string System { get; set; }
        public int Priority { get; set; }
        public string SearchValue { get; set; }
        public Guid SspArea_Id { get; set; }
        [ForeignKey("SspArea_Id")]
        public SspArea SspArea { get; set; }
    }
}
