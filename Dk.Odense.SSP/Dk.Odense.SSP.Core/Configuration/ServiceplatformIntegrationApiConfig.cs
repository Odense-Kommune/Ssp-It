using System;

namespace Dk.Odense.SSP.Core.Configuration
{
    public class ServiceplatformIntegrationApiConfig
    {
        public string Endpoint { get; set; }
        public Guid ServiceAgreement { get; set; }
        public Guid SystemUser { get; set; }
        public string CertificateThumbprint { get; set; }
    }
}
