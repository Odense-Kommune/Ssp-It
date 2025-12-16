namespace Dk.Odense.SSP.SagOgDokIndeks.Config
{
    internal class AppSettings
    {
        public SODIConfig SODIConfig { get; set; } = new SODIConfig();
        public LogSettings LogSettings { get; set; } = new LogSettings();
    }
}
