namespace Dk.Odense.SSP.SagOgDokIndeks.Config
{
    internal class SODIConfig
    {
        //WSC
        public string WscCertificateThumbprint { get; set; }

        //WSP
        public string WspEndpointBaseUrl { get; set; }
        public string WspEndpointID { get; set; }
        public string WspCertificateThumbprint { get; set; }

        //STS
        public string StsEndpointAddress { get; set; }
        public string StsEntityIdentifier { get; set; }
        public string StsCertificateThumbprint { get; set; }

        public string Cvr { get; set; }
        public string CvrUuid { get; set; }
        public bool TrustAllCertificates { get; set; }
        public string Environment { get; set; }
    }
}
