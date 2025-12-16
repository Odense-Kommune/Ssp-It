using Dk.Odense.SSP.Core;

namespace Dk.Odense.SSP.UserCase.ViewModel
{
    public class CollapseMenuItem : Entity
    {
        public string Name { get; set; }
        public string SocialSecNum { get; set; }
        public int WorriesCount { get; set; }
        public string SspArea { get; set; }
    }
}
