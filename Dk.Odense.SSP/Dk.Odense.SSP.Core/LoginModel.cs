using System;
using System.Collections.Generic;

namespace Dk.Odense.SSP.Core
{
    public class LoginModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string SamlId { get; set; }
        public string CprNumberIdentifier { get; set; } // dk:gov:saml:attribute:CprNumberIdentifier

        public string UserName { get; set; } // urn:oid:0.9.2342.19200300.100.1.1
        public string Email { get; set; } // urn:oid:0.9.2342.19200300.100.1.3

        public string Position { get; set; } // urn:oid:2.5.4.12
        public string FullName { get; set; } // urn:oid:2.5.4.3

        public ICollection<string> Roles { get; set; }

        public DateTime Created { get; set; }

        public string MunicipalityDepartment { get; set; }
        public string SbsysToken { get; set; }
    }
}
