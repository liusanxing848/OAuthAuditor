using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.Core.Objects
{
    public class AccountGroup
    {
        public string id;
        public string name;
        public string categoryId;
        public string state;
        public string description;
        public List<int> bnetAccountMembers;
        public List<int> bnetAccountOwners;
        public List<BnetAccount> bnetAccountMembers_obj;
        public List<BnetAccount> bnetAccountOwners_obj;
        public List<string> oauthClientOwners;
        public List<OAuthClient> oauthClientOwners_obj;
        public List<string> scopes;

    }
}
