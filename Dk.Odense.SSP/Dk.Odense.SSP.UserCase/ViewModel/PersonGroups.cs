using Dk.Odense.SSP.Domain.Model;
using System;
using System.Collections.Generic;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class PersonGroups
    {
        public PersonGroups()
        {
            groups = new List<Grouping>();
        }
        public List<Grouping> groups { get; set; }
        public string Name { get; set; }
        public string cpr { get; set; }
        public Guid Person_Id { get; set; }
        public Guid Classification_Id { get; set; }
    }
}
