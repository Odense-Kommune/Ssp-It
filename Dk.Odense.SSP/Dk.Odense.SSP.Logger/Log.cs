using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dk.Odense.SSP.Logger
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string User { get; set; }
        public string Method { get; set; }
        public string RequestParm { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
