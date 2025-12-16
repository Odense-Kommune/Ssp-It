using System;
using System.Collections.Generic;
using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SocialSecNum { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        public string SocialWorker { get; set; }
        public IEnumerable<Grouping> Groupings { get; set; }
        public int WorriesCount { get; set; }
        public GuidString SspArea { get; set; }
        public GuidString Categorization { get; set; }
        public GuidString AgendaCategorization { get; set; }
        public SchoolDataDTO SchoolData { get; set; }
        public DateTime? SspStopDate { get; set; }
        public IEnumerable<int> WorryIncrements { get; set; }
    }
}
