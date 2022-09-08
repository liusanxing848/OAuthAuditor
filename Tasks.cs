using AccessAdminAuditorV3.App;
using AccessAdminAuditorV3.App.IO;
using AccessAdminAuditorV3.Core;
using AccessAdminAuditorV3.Core.Objects;
using AccessAdminAuditorV3.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using static AccessAdminAuditorV3.Core.Service.AccountGroupServices;
using static AccessAdminAuditorV3.Core.Service.AuthorizationServices;
using static AccessAdminAuditorV3.Core.Service.BattlenetAccountServices;
using static AccessAdminAuditorV3.Core.Service.ExternalIdentityServices;

namespace AccessAdminAuditorV3
{
    class Tasks
    {
        /// <summary>
        /// Call Tasks.Initialize to load all endpoints, Client Details, and call token.
        /// Will print out all specs
        /// </summary>
        public static void Initialize()
        {
            FileManager.LoadEndpoints(Paths.defaultEndpoints);
            FileManager.LoadOAuthClients(Paths.defaultClients);
            TokenServices.GetAllClientsToken();
        }

        public class AccountGroupServices
        {
            public static string GetAccountGroupInfo_SingleAPICall(TaskConfiguration c, string accountGroupId)
            {
                return new GetAccountGroup(c).GetResult_SimpleAPICall(accountGroupId);
            }

            public static AccountGroup GetFullAccountGroupInfo_FullObjDisplay(TaskConfiguration c, string accountGroupId)
            {
                return new GetAccountGroup(c).CreateFullAccountGroupObjectFromAccountGroupId(accountGroupId);
            }

            public static void ExportFullAccountGroupDetailstoCSV_FromReadingFile(TaskConfiguration c, string importFilePath, string exportFileName)
            {
                Core.Service.AccountGroupServices.ExportAccountGroupAuditingFromImportingFile(c, importFilePath, exportFileName);
                for (int i = 0; i < 10; i++)
                {
                    Display.DisplayGreen("EXPORT COMPLETED!");
                    System.Threading.Thread.Sleep(200);
                }
            }
            public static string GetAccountGroupDefinition_Integrated(TaskConfiguration c)
            {
                return new GetAccountGroupDefinitions(c).GetDefinition_IntegratedAPICall();
            }
        }

        public class BattlenetAccountServices
        {
            public static string GetAccountInfo_SingleAPICall(TaskConfiguration c, int accountId)
            {
                return new GetAccountInfo(c).GetResult_SimpleAPICall(accountId);
            }

            public static void GetAccountInfo_DisplayIntegratedObjInfo(TaskConfiguration c, int accountId)
            {
                new GetAccountInfo(c).DisplayBnetInfoFromId(accountId);
            }
   
        }

        public class AuthorizationServices
        {
            public static string GetScopeForRole_SingleAPICall(TaskConfiguration c, string accountGroupId)
            {
                return new GetScopesForRole(c).GetResult_SimpleAPICall(accountGroupId);
            }
            public static string GetOAuthClient_SingleAPICall(TaskConfiguration c, string clientId)
            {
                return new GetClient(c).GetResult_SimpleAPICall(clientId);
            }
            public static OAuthClient GetOAuthClientDetailFromClientId(TaskConfiguration c, string clientId)
            {
                return new GetClient(c).GetOAthClientDetailFromClientId(clientId);
            }
            public static string GetClientsListFromScope(TaskConfiguration c, string scope)
            {
                return new GetClientIdByScope(c).GetResult_SimpleAPICall(scope);
            }
            public static void WhoIsUsingMyScope(TaskConfiguration c, string scope, string exportFileName)
            {
                Core.Service.AuthorizationServices.WhoIsUsingMyScope(c, scope, exportFileName);
                for(int i = 0; i < 10; i++)
                {
                    Display.DisplayGreen("EXPORT COMPLETED!");
                    System.Threading.Thread.Sleep(200);
                }
            }

        }

        public class ExternalIdentiyServices
        {
            public static string GetBnetAccountInfoFromEmail_SingleAPICall(TaskConfiguration c, string email)
            {
                return new GetAccountIdByExternalIdentity(c).GetResult_SimpleAPICall(email);
            }

            public static BnetAccount GetFullBnetAccountInfoFromEmail(TaskConfiguration c, string email)
            {
                return new GetAccountIdByExternalIdentity(c).getBnetAccountObjFromEmail(email);
            }

            public static void GetBnetAccountDetailFromEmailDisplay(TaskConfiguration c, string email)
            {
                BnetAccount acc = GetFullBnetAccountInfoFromEmail(c, email);
                
            }
        }
    }
}
