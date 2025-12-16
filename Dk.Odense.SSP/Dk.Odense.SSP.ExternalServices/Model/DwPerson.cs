namespace Dk.Odense.SSP.ExternalServices.Model
{
    public class DwPerson
    {
        public string Uuid { get; set; }
        public string Pnr { get; set; }
        public string Lastname { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double PostalCode { get; set; }
        public string PostalTxt { get; set; }
        public bool Subscribed { get; set; }
    }
}
