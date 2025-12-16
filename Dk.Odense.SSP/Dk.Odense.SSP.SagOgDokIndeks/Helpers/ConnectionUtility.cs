using Digst.OioIdws.CommonCore;
using Digst.OioIdws.OioWsTrustCore;
using Digst.OioIdws.SoapCore;
using Digst.OioIdws.SoapCore.Bindings;
using Digst.OioIdws.SoapCore.Tokens;
using Digst.OioIdws.WscCore.OioWsTrust;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Dk.Odense.SSP.SagOgDokIndeks.Helpers
{
    internal class ConnectionUtility
    {
        public static PortType CreateChannel<PortType>(string service, string operation)
        {

            var wscConfiguration = new OioIdwsWcfConfigurationSection
            {
                StsEndpointAddress = OrganisationRegistryProperties.AppSettings.SODIConfig.StsEndpointAddress,
                StsEntityIdentifier = OrganisationRegistryProperties.AppSettings.SODIConfig.StsEntityIdentifier,

                StsCertificate = new Certificate
                {
                    StoreName = StoreName.My,
                    StoreLocation = StoreLocation.LocalMachine,
                    X509FindType = X509FindType.FindByThumbprint,
                    FindValue = OrganisationRegistryProperties.AppSettings.SODIConfig.StsCertificateThumbprint,
                },
                ServiceCertificate = new Certificate
                {
                    StoreName = StoreName.My,
                    StoreLocation = StoreLocation.LocalMachine,
                    X509FindType = X509FindType.FindByThumbprint,
                    FindValue = OrganisationRegistryProperties.AppSettings.SODIConfig.WspCertificateThumbprint,
                },
                WspEndpoint = OrganisationRegistryProperties.AppSettings.SODIConfig.WspEndpointBaseUrl + service + "/",
                WspEndpointID = OrganisationRegistryProperties.AppSettings.SODIConfig.WspEndpointID,
                WspSoapVersion = "1.2",


                ClientCertificate = new Certificate
                {
                    StoreName = StoreName.My,
                    StoreLocation = StoreLocation.LocalMachine,
                    X509FindType = X509FindType.FindByThumbprint,
                    FindValue = OrganisationRegistryProperties.AppSettings.SODIConfig.WscCertificateThumbprint,
                },

                Cvr = OrganisationRegistryProperties.AppSettings.SODIConfig.Cvr,
                TokenLifeTimeInMinutes = 120,
                IncludeLibertyHeader = false,
                MaxReceivedMessageSize = Int32.MaxValue,
            };

            var stsConfiguration = TokenServiceConfigurationFactory.CreateConfiguration(wscConfiguration);
            // OrganisationRegistryProperties.Init();
            if (OrganisationRegistryProperties.AppSettings.SODIConfig.TrustAllCertificates)
            {
                stsConfiguration.SslCertificateAuthentication.RevocationMode = X509RevocationMode.NoCheck;
                stsConfiguration.StsCertificateAuthentication.RevocationMode = X509RevocationMode.NoCheck;
                stsConfiguration.WspCertificateAuthentication.RevocationMode = X509RevocationMode.NoCheck;

                stsConfiguration.SslCertificateAuthentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                stsConfiguration.StsCertificateAuthentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                stsConfiguration.WspCertificateAuthentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
            }
            IStsTokenService stsTokenService = new StsTokenServiceCache(stsConfiguration);
            var securityToken = (GenericXmlSecurityToken)stsTokenService.GetToken();

            return CreateChannelWithIssuedToken<PortType>(securityToken, stsConfiguration, service, operation);
            
        }
        public static T CreateChannelWithIssuedToken<T>(GenericXmlSecurityToken token, StsTokenServiceConfiguration stsConfiguration, string service, string operation)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            if (stsConfiguration == null)
                throw new ArgumentNullException(nameof(stsConfiguration));

            // IMPORTANT: https://devblogs.microsoft.com/dotnet/wsfederationhttpbinding-in-net-standard-wcf/
            // First, create the inner binding for communicating with the token issuer.
            // The security settings will be specific to the STS and should mirror what
            // would have been in an app.config in a .NET Framework scenario.

            var serverCertificate = stsConfiguration.WspConfiguration.ServiceCertificate;
            var messageVersion = MessageVersion.CreateVersion(stsConfiguration.WspConfiguration.SoapVersion, AddressingVersion.WSAddressing10);

            // Create a token parameters. The token is then used by FederatedChannelSecurityTokenManager to create an instance of FederatedTokenSecurityTokenProvider which returns the token immediately
            var tokenParameters = new FederatedSecurityTokenParameters(token, messageVersion, stsConfiguration, stsConfiguration.WspConfiguration.EndpointAddress)
            {
                MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10,
                MaxReceivedMessageSize = stsConfiguration.MaxReceivedMessageSize,
                IncludeLibertyHeader = stsConfiguration.IncludeLibertyHeader,
            };

            var bindingToCallService = new OioIdwsSoapBinding(tokenParameters);
            FederatedChannelFactory<T> factory = CreateFactory<T>(stsConfiguration, serverCertificate, bindingToCallService);

            /*  if (OrganisationRegistryProperties.AppSettings.LogSettings.LogRequestResponse)
              {
                  factory.Endpoint.EndpointBehaviors.Add(new LoggingBehavior(service, operation));
              }*/

            factory.Endpoint.EndpointBehaviors.Add(new RequestHeaderBehavior());

            // .NET Core does not support asymmetric binding, so it does not call the CreateSecurityTokenAuthenticator method to create an X509SecurityTokenAuthenticator to validate the service certificate
            // Implement a custom X509SecurityTokenAuthenticator is not an option because not all necessary types used by that abstract class is exposed to .NET Core
            stsConfiguration.WspCertificateAuthentication.Validate(serverCertificate);

            return factory.CreateChannel();
        }
        private static FederatedChannelFactory<T> CreateFactory<T>(IStsTokenServiceConfiguration stsConfiguration, X509Certificate2 serverCertificate, OioIdwsSoapBinding bindingToCallService)
        {
            // we need to create a client 
            var factory = new FederatedChannelFactory<T>(bindingToCallService, new EndpointAddress(stsConfiguration.WspConfiguration.EndpointAddress));
            factory.Credentials.ServiceCertificate.Authentication.CopyFrom(stsConfiguration.WspCertificateAuthentication);
            factory.Credentials.ServiceCertificate.SslCertificateAuthentication = stsConfiguration.SslCertificateAuthentication.DeepClone();

            string dnsName = serverCertificate.GetNameInfo(X509NameType.DnsName, false);
            EndpointIdentity identity = new DnsEndpointIdentity(dnsName);
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(stsConfiguration.WspConfiguration.EndpointAddress), identity);
            factory.Endpoint.Address = endpointAddress;
            factory.Credentials.ClientCertificate.Certificate = stsConfiguration.ClientCertificate;
            factory.Credentials.ServiceCertificate.ScopedCertificates.Add(endpointAddress.Uri, serverCertificate);
            return factory;
        }

    }
}