namespace Dk.Odense.SSP.Core.Configuration
{
    public class DevelopmentSettingsConfig
    {
        public bool IsDevelopment { get; set; }

        //Note: Move this to a helper class later (or use a constructor), so we can set IsDevelopment to private
        public bool CheckIfDevelopment()
        {
            return IsDevelopment;
        }
    }
}
