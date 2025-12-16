using System;

namespace Dk.Odense.SSP.ExternalServices.Model
{
    public class SpPerson
    {
        public Guid Guid { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Sex { get; set; }
        public string CityName { get; set; }
        public string CommuneCode { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string StreetCode { get; set; }
        public string PostalDistrict { get; set; }
        public string Status { get; set; }
    }
}
