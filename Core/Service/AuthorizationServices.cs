using AccessAdminAuditorV3.App;
using AccessAdminAuditorV3.App.IO;
using AccessAdminAuditorV3.Core.API;
using AccessAdminAuditorV3.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static AccessAdminAuditorV3.App.TaskConfiguration;

namespace AccessAdminAuditorV3.Core.Service
{
    public class AuthorizationServices
    {
        public static void WhoIsUsingMyScope(TaskConfiguration c, string scope, string exportFileName)
        {
            List<string> clientIdList = new GetClientIdByScope(c).GetOauthClientIdListFromScope(scope);
            List<OAuthClient> OAuthClientObjList = new GetClient(c).GetListOfOAuthClientObjFromClientIdList(clientIdList);
            Utility._WriteToCSVFromOAuthClientObjectList(OAuthClientObjList, exportFileName, scope);
        }
        public class GetScopesForRole
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private APIClient _client;
            private TaskConfiguration _c;

            public GetScopesForRole(TaskConfiguration config)
            {
                _c = config;
                _ServiceInit(_c);
            }

            private void _ServiceInit(TaskConfiguration config)
            {
                if (config.env == Config.Production)
                {
                    _client = Data.Clients.prodOAuthClient;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.Production.Auth.AuthorizationService.getScopesForRole;
                    }
                }
                if (config.env == Config.QA)
                {
                    _client = Data.Clients.qaOAuthClient;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Auth.AuthorizationService.getScopesForRole;
                    }
                }

            }

            private HttpContent _HttpContent (string id)
            {
                var body = new
                {
                    accountRole = id
                };
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }

            //[API]
            public string GetResult_SimpleAPICall(string accountGroupId)
            {
                HttpRequestMessage requestMessage = RequestMessages.requestMessage_Auth(_method, _HttpContent(accountGroupId), _client.id, _client.secret, _uri);
                Console.WriteLine($"Getting Scopes info from account group: {accountGroupId}");
                return API.API.GetResult(requestMessage);
            }

            //[Parse]
            public List<string> ParseScopetoListFromResponse(string response)
            {
                List<string> scopes = new List<string>();
                JObject jo = JObject.Parse(response);
                int length = 0;
                try
                {
                    while (jo["scopes"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                for (int i = 0; i < length; i++)
                {
                    scopes.Add(jo["scopes"][i].ToString());
                }
                return scopes;
            }

            //[API] + [Parse]
            public List<string> GetScopeListFromAccountGroupId(string id)
            {
                return ParseScopetoListFromResponse(GetResult_SimpleAPICall(id));
            }
        }

        public class GetClientIdByScope
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private APIClient _client;
            private TaskConfiguration _c;

            public GetClientIdByScope(TaskConfiguration config)
            {
                _c = config;
                _ServiceInit(_c);
            }

            private void _ServiceInit(TaskConfiguration config)
            {
                if (config.env == Config.Production)
                {
                    _client = Data.Clients.prodOAuthClient;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.Production.Auth.AuthorizationService.getClientIdByScope;
                    }
                }
                if (config.env == Config.QA)
                {
                    _client = Data.Clients.qaOAuthClient;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Auth.AuthorizationService.getClientIdByScope;
                    }
                }

            }

            private HttpContent _HttpContent(string scope)
            {
                var body = new
                {
                    scope = scope
                };
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }

            //[API]
            public string GetResult_SimpleAPICall(string scope)
            {
                HttpRequestMessage requestMessage = RequestMessages.requestMessage_Auth(_method, _HttpContent(scope), _client.id, _client.secret, _uri);
                Console.WriteLine($"Fecthing OAuth Clients for scope: {scope}");
                return API.API.GetResult(requestMessage);
            }

            //[Parse]
            public List<string> GetOauthClientIdListFromRespond(string response)
            {
                List<string> res = new List<string>();
                JObject jo = JObject.Parse(response);

                int length = 0;
                try
                {
                    while (jo["clientIds"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }

                for (int i = 0; i < length; i++)
                {
                    res.Add(jo["clientIds"][i].ToString());
                }
                return res;
            }

            //[API] [Parse] [Hybrid]
            public List<string> GetOauthClientIdListFromScope(string scope)
            {
                string resp = GetResult_SimpleAPICall(scope);
                return GetOauthClientIdListFromRespond(resp);
            }
        }

        public class GetClient
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private APIClient _client;
            private TaskConfiguration _c;

            public GetClient(TaskConfiguration config)
            {
                _c = config;
                _ServiceInit(_c);
            }

            private void _ServiceInit(TaskConfiguration config)
            {
                if (config.env == Config.Production)
                {
                    _client = Data.Clients.prodOAuthClient;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.Production.Auth.AuthorizationService.getClient;
                    }
                }
                if (config.env == Config.QA)
                {
                    _client = Data.Clients.qaOAuthClient;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Auth.AuthorizationService.getClient;
                    }
                }

            }

            private HttpContent _HttpContent(string clientId)
            {
                var body = new
                {
                    clientId = clientId
                };
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }

            //[API]
            public string GetResult_SimpleAPICall(string clientId)
            {
                HttpRequestMessage requestMessage = RequestMessages.requestMessage_Auth(_method, _HttpContent(clientId), _client.id, _client.secret, _uri);
                Console.WriteLine($"Getting Oauth Client Detail for client: {clientId}");
                return API.API.GetResult(requestMessage);
            }

            //[API] [Batch] [Threading]
            public List<string> GetResult_BatchAPICall(List<string> clientIdList)
            {
                List<string> list = new List<string>();
                Parallel.ForEach(clientIdList, id =>
                {
                    list.Add(GetResult_SimpleAPICall(id));
                });

                return list;
            }

            //[Parse]
            public OAuthClient ParseOAuthClientDetailtoObjectFromResponse(string resp)
            {
                OAuthClient client = new OAuthClient();
                JObject jo = JObject.Parse(resp);
                client.clientId = jo["client"]["clientId"].ToString();
                client.ownerEmail = jo["client"]["ownerEmail"].ToString();
                client.clientName = jo["client"]["clientName"].ToString();
                client.intendedUse = jo["client"]["intendedUse"].ToString();
                client.scopes = _ParseScopesFromResponse(resp);
                client.clientRoles = _ParseClientRolesFromResponse(resp);
                client.authorizedGrantTypes = _ParseAuthorizationGrantTypesFromResponse(resp);
                client.owner_obj = new ExternalIdentityServices.GetAccountIdByExternalIdentity(_c).getBnetAccountObjFromEmail(client.ownerEmail);
                client.ownerAccountId = client.owner_obj.id;
                DisplayFullDetailOfOAuthClient(client);

                return client;
            }

            //[Parse] [Batch]
            public List<OAuthClient> GetListOfOAuthClientObjFromClientIdList(List<string> clientIdList)
            {
                List<string> resp = GetResult_BatchAPICall(clientIdList);
                List<OAuthClient> list = new List<OAuthClient>();
                foreach(string r in resp)
                {
                    list.Add(ParseOAuthClientDetailtoObjectFromResponse(r));
                }
                return list;
            }

            //[API][Parse][Hybrid]
            public OAuthClient GetOAthClientDetailFromClientId(string clientId)
            {
                string resp = GetResult_SimpleAPICall(clientId);
                return ParseOAuthClientDetailtoObjectFromResponse(resp);
            }

            //[Parse]
            private List<string> _ParseScopesFromResponse(string resp)
            {
                
                JObject jo = JObject.Parse(resp);
                List<string> list = new List<string>();

                int length = 0;
                try
                {
                    while (jo["client"]["scopes"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                
                for (int i = 0; i < length; i++)
                {
                    list.Add(jo["client"]["scopes"][i].ToString());
                }
                return list;
            }

            //[Parse]
            private List<string> _ParseClientRolesFromResponse(string resp)
            {
                JObject jo = JObject.Parse(resp);
                List<string> list = new List<string>();

                int length = 0;
                try
                {
                    while (jo["client"]["clientRoles"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                for (int i = 0; i < length; i++)
                {
                    list.Add(jo["client"]["clientRoles"][i].ToString());
                }
                return list;
            }

            //[Parse]
            private List<string> _ParseAuthorizationGrantTypesFromResponse(string resp)
            {
                JObject jo = JObject.Parse(resp);
                List<string> list = new List<string>();

                int length = 0;
                try
                {
                    while (jo["client"]["authorizedGrantTypes"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                for (int i = 0; i < length; i++)
                {
                    list.Add(jo["client"]["authorizedGrantTypes"][i].ToString());
                }
                return list;
            }

            //[Display]
            public static void DisplayFullDetailOfOAuthClient(OAuthClient c)
            {
                Display.DisplayGreen("__________________________________________________________________________");
                Console.Write("ClientId: ");
                Display.DisplayYellow(c.clientId);
                Console.Write("Client Name: ");
                Display.DisplayYellow(c.clientName);
                Console.Write("Intend to Use: ");
                Display.DisplayYellow(c.intendedUse);
                Console.Write("Authorization Grant Types: ");
                foreach(string type in c.authorizedGrantTypes)
                {
                    Display.DisplayYellow(type);
                }
                Console.Write("Client Roles: ");
                foreach(string role in c.clientRoles)
                {
                    Display.DisplayYellow(role);
                }
                Console.Write("Scopes: ");
                foreach(string scope in c.scopes)
                {
                    Display.DisplayYellow(scope);
                }
                Console.WriteLine("Owner Infos:");
                BattlenetAccountServices.GetAccountInfo.DisplayBnetAccountObj(c.owner_obj);

            }
        }

        public class Utility
        {
            private static DataTable _OAuthClientObjectToDataTable(List<OAuthClient> OAuthClientList, string scope = null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(" ");
                dt.Columns.Add("  ");
                dt.Columns.Add("   ");
                dt.Columns.Add("    ");

                if(scope != null)
                {
                    dt.Rows.Add($"Auditing Scope---> {scope}");
                    dt.Rows.Add("");
                    dt.Rows.Add("");
                    dt.Rows.Add("");
                }

                foreach(OAuthClient c in OAuthClientList)
                {
                    string[] clientId = new string[] { "OAuth Client ID: ", c.clientId };
                    string[] clientName = new string[] { "OAuth Client Name: ", c.clientName };
                    string[] intentToUse = new string[] { "Intented To Use: ", c.intendedUse };
                    string[] ownerEmail = new string[] { "Owner Email: ", c.ownerEmail };
                    dt.Rows.Add(clientId);
                    dt.Rows.Add(clientName);
                    dt.Rows.Add(intentToUse);
                    dt.Rows.Add(ownerEmail);
         

          
                    string[] ownerId = new string[] { "Owner account ID:", c.owner_obj.id.ToString() };
                    string[] ownerName = new string[] {"Owner Name:", $"{c.owner_obj.firstName} {c.owner_obj.lastName}" };
                    dt.Rows.Add(ownerId);
                    dt.Rows.Add(ownerName);
                    dt.Rows.Add("");

                    /*
                    dt.Rows.Add("Authorization Grant Type:");
                    foreach(string type in c.authorizedGrantTypes)
                    {
                        dt.Rows.Add(type);
                    }
                    dt.Rows.Add("");

                    dt.Rows.Add("Client Roles:");
                    foreach(string role in c.clientRoles)
                    {
                        dt.Rows.Add(role);
                    }
                    dt.Rows.Add("");

                    dt.Rows.Add("Client Scopes:");
                    foreach(string s in c.scopes)
                    {
                        dt.Rows.Add(s);
                    }
                    dt.Rows.Add("");
                    */
                }
                return dt;
            }
            private static void _WriteToCSVFromDataTable(DataTable dt, string outputFileName)
            {
                FileManager.WriteToCSV_FromDataTable(dt, outputFileName);
            }
            internal static void _WriteToCSVFromOAuthClientObjectList(List<OAuthClient> OAuthClientList, string outputFileName, string scope = null)
            {
                DataTable dt = _OAuthClientObjectToDataTable(OAuthClientList, scope);
                _WriteToCSVFromDataTable(dt, outputFileName);
            }
        }
    }
}
