using System;

namespace Dk.Odense.SSP.Sbsys.Model
{
    public class SbsysCase
    {
        public Guid Id { get; set; }
        public string Sagsstatus { get; set; }
        public string Sagstilstand { get; set; }
        public string SagSkabelonId { get; set; }
    }
}
