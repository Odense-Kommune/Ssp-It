namespace Dk.Odense.SSP.Core
{
    public class NavneOpslagData
    {
        public NavneOpslagData() { }

        public NavneOpslagData(string name, string address, string socialSecNum, string birthday)
        {
            Name = name;
            Address = address;
            SocialSecNum = socialSecNum;
            Birthday = birthday;
        }

        public string SocialSecNum { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }
    }
}
