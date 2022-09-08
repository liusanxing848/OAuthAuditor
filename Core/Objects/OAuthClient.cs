using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.Core.Objects
{
    public class OAuthClient
    {
        public string clientId;
        public string ownerEmail;
        public string intendedUse;
        public string clientName;
        public BnetAccount owner_obj;
        public List<string> scopes;
        public List<string> authorizedGrantTypes;
        public List<string> clientRoles;
        public int ownerAccountId;
    }
}
