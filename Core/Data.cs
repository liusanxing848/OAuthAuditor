using AccessAdminAuditorV3.Core.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.Core
{
    public static class Data
    {
        public static class Endpoints
        {
            public static class QA
            {
                public static class TokenService
                {
                    public static string getToken;
                }
                public static class Asterion
                {
                    public static string baseURI;
                    public static class BattleNetAccountService
                    {
                        public static string baseURI;

                        public static string getAccountInfo;


                    }
                    public static class AccountGroupService
                    {
                        public static string baseURI;

                        public static string getCategoryDefinitions;
                        public static string getAccountGroupsByCategory;
                        public static string getAccountGroup;
                        public static string getMembersForGroup;
                    }
                    public static class ExternalIdentityService
                    {
                        public static string baseURI;

                        public static string getAccountIdByExternalIdentiy;
                    }
                }

                public static class Auth
                {
                    public static string baseURI;
                    public static class AuthorizationService
                    {
                        public static string baseURI;

                        public static string getScope;
                        public static string getClientIdByScope;
                        public static string getClient;
                        public static string getScopesForRole;
                    }
                }

                public static class Gateway
                {
                    public static string baseURI;
                }

            }

            public static class Production
            {
                public static class TokenService
                {
                    public static string getToken;
                }
                public static class Asterion
                {
                    public static string baseURI;
                    public static class BattleNetAccountService
                    {
                        public static string baseURI;

                        public static string getAccountInfo;


                    }
                    public static class AccountGroupService
                    {
                        public static string baseURI;

                        public static string getCategoryDefinitions;
                        public static string getAccountGroupsByCategory;
                        public static string getAccountGroup;
                        public static string getMembersForGroup;
                    }
                    public static class ExternalIdentityService
                    {
                        public static string baseURI;

                        public static string getAccountIdByExternalIdentiy;
                    }
                }

                public static class Auth
                {
                    public static string baseURI;
                    public static class AuthorizationService
                    {
                        public static string baseURI;

                        public static string getScope;
                        public static string getClientIdByScope;
                        public static string getClient;
                        public static string getScopesForRole;
                    }
                }
                public static class Gateway
                {
                    public static string baseURI;
                }
            }



        }

        public static class Clients
        {
            public static APIClient qaOAuthClient = new APIClient("Auditor (QA)");
            public static APIClient prodOAuthClient = new APIClient("Auditor (Production)");         
        }
    }
}
