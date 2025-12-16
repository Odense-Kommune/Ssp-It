using System;

namespace Dk.Odense.SSP.Core
{
    public class DeletePerson
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int WorryCount { get; set; }
        public DateTime? SspStopDate { get; set; }
        public DateTime? LatestWorryDate { get; set; }
        public int? LatestCategorizationDeleteAfter { get; set; }
        public bool? DelteAfterSspEnd { get; set; }
    }
}
