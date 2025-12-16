using System.Collections.Generic;

namespace Dk.Odense.SSP.Core.Configuration
{
    public class MailConfig
    {
        public string Smtp { get; set; }
        public int SmtpPort { get; set; }
        public string FromMail { get; set; }
        public List<string> Receivers { get; set; }
    }
}