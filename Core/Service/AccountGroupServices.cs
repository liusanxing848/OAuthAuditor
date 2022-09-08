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
    public class AccountGroupServices
    {

        //[Batch] [METHOD]
        public static void ExportAccountGroupAuditingFromImportingFile(TaskConfiguration c, string importFilePath, string exportFileName)
        {
            List<string> agIdList = FileManager.ReadCSVtoList(importFilePath);
            List<AccountGroup> accountGroupObjList = new GetAccountGroup(c).CreateFullAccountGroupObjListFromAccountGroupId_Batch(agIdList);
            Utility.WriteToCSVFromAccountGroupObjList(accountGroupObjList, exportFileName);
        }
        public class GetAccountGroup
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private string _token;
            private TaskConfiguration _c;
            
            /// <summary>
            /// Constructor 
            /// This constructor for single call
            /// </summary>
            /// <param name="accountGroupId"></param>
            /// <param name="config"></param>
            public GetAccountGroup(TaskConfiguration config)
            {
                _c = config;
                _ServiceInit(_c);
            }

            private void _ServiceInit(TaskConfiguration config)
            {
                if(config.env == Config.Production)
                {
                    _token = Data.Clients.prodOAuthClient.token;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.Production.Asterion.AccountGroupService.getAccountGroup;
                    }
                }
                if(config.env == Config.QA)
                {
                    _token = Data.Clients.qaOAuthClient.token;
                    if(config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Asterion.AccountGroupService.getAccountGroup;
                    }
                }

            }
            private HttpContent _HttpContent(string id)
            {
                var body = new
                {
                    accountGroupId = id,
                    includeOwners = true
                };
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }

            //[API] single Call
            public string GetResult_SimpleAPICall(string id)
            {
                HttpRequestMessage requestMessage = RequestMessages.requestMessage(_method, _HttpContent(id), _uri);
                
                Console.WriteLine($"Getting info for Account Group: {id}");
                return API.API.GetResult(requestMessage, _token);
            }

            //[API] Batch Call, multi-threading
            internal List<string> _GetResult_BatchCall(List<string> accounGroupIdList)
            {
                List<string> resp = new List<string>();
                
                Parallel.ForEach(accounGroupIdList, id =>
                {
                    resp.Add(GetResult_SimpleAPICall(id));
                });
                return resp;
            }

            //-----Anything Account Group Object
            private AccountGroup _CreateFullAccountGroupObjFromResponse(string response)
            {
                AccountGroup ag = new AccountGroup();
                JObject jo = JObject.Parse(response);
                ag.id = jo["accountGroup"]["accountGroupId"].ToString();
                ag.bnetAccountOwners = _GetAccountGroupBnetAccountOwnerList(response);
                ag.categoryId = jo["accountGroup"]["categoryId"].ToString();
                ag.name = jo["accountGroup"]["name"].ToString();
                ag.state = jo["accountGroup"]["state"].ToString();
                ag.bnetAccountMembers = new GetMembersForGroup(_c)._AccountMemberIdList_FromAccountGroupId(ag.id);
                ag.scopes = new AuthorizationServices.GetScopesForRole(_c).GetScopeListFromAccountGroupId(ag.id);
                ag.bnetAccountOwners_obj = new BattlenetAccountServices.GetAccountInfo(_c).CreateBnetAccountObjListFromAccountId_Batch(ag.bnetAccountOwners);
                ag.bnetAccountMembers_obj = new BattlenetAccountServices.GetAccountInfo(_c).CreateBnetAccountObjListFromAccountId_Batch(ag.bnetAccountMembers);
                _DisplayAccountGroupObjectDetail(ag);
                
                return ag;
            }

            
            private List<AccountGroup> _CreateFullAccountGroupObjListFromResponse_Batch(List<string> responsesList)
            {
                List<AccountGroup> agList = new List<AccountGroup>();
                foreach(string resp in responsesList)
                {
                    agList.Add(_CreateFullAccountGroupObjFromResponse(resp));
                }
                return agList;
            }

            public AccountGroup CreateFullAccountGroupObjectFromAccountGroupId(string id)
            {
                string resp = GetResult_SimpleAPICall(id);
                return _CreateFullAccountGroupObjFromResponse(resp);
            }

            public List<AccountGroup> CreateFullAccountGroupObjListFromAccountGroupId_Batch(List<string> Ids)
            {
                List<AccountGroup> list = new List<AccountGroup>();
                foreach(string id in Ids)
                {
                    list.Add(CreateFullAccountGroupObjectFromAccountGroupId(id));
                }
                return list;
            }
            
            //-------------

            private List<int> _GetAccountGroupBnetAccountOwnerList(string response)
            {
                List<int> owners = new List<int>();
                JObject jo = JObject.Parse(response);
                string groupID = jo["accountGroup"]["accountGroupId"].ToString();

                int length = 0;
                try
                {
                    while (jo["accountGroup"]["ownershipDetails"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                for (int i = 0; i < length; i++)
                {
                    if (jo["accountGroup"]["ownershipDetails"][i]["ownerTypeId"].ToString() == "2")
                    {
                        int ownerId = Int32.Parse(jo["accountGroup"]["ownershipDetails"][i]["ownershipId"].ToString());
                        owners.Add(ownerId);
                    }
                }
                return owners;
            }

            internal void _DisplayAccountGroupObjectDetail(AccountGroup ag)
            {
                Display.DisplayGreen("_______________________________________________________________"); 
                Console.Write("AccountGroupId: ");
                Display.DisplayYellow(ag.id);
                Console.Write("AccountGroupCategory: ");
                Display.DisplayYellow(ag.categoryId);
                Console.Write("AccountGroupName: ");
                Display.DisplayYellow(ag.name);
                Console.Write("state: ");
                Display.DisplayYellow(ag.state);

                Console.WriteLine("Owners:");
                foreach (BnetAccount acc in ag.bnetAccountOwners_obj)
                {
                    try
                    {
                        Console.Write("Info:");
                        Display.DisplayYellow($"{acc.id} -- {acc.firstName} {acc.lastName}, {acc.email}, {acc.battleTag}");
                    }
                    catch(NullReferenceException)
                    {
                        Display.DisplayGreen("Empty");
                    }
                    
                    
                }
                Console.WriteLine("AccountGroupMember:");
                foreach (BnetAccount acc in ag.bnetAccountMembers_obj)
                {
                    try
                    {
                        Console.Write("Info:");
                        Display.DisplayYellow($"{acc.id} -- {acc.firstName} {acc.lastName}, {acc.email}, {acc.battleTag}");
                    }
                    catch (NullReferenceException)
                    {
                        Display.DisplayGreen("Empty");
                    }
                }

                foreach (string scope in ag.scopes)
                {
                    try
                    {
                        Console.Write("Scope: ");
                        Display.DisplayYellow(scope);
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("Empty");
                    }

                }
            }

        }
        public class GetMembersForGroup
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private string _token;
            private TaskConfiguration _c;

            //ctor
            public GetMembersForGroup(TaskConfiguration config)
            {
                _c = config;
                _ServiceInit(_c);
            }

            private void _ServiceInit(TaskConfiguration config)
            {
                if (config.env == Config.Production)
                {
                    _token = Data.Clients.prodOAuthClient.token;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.Production.Asterion.AccountGroupService.getMembersForGroup;
                    }
                }
                if (config.env == Config.QA)
                {
                    _token = Data.Clients.qaOAuthClient.token;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Asterion.AccountGroupService.getMembersForGroup;
                    }
                }

            }

            private HttpContent _HttpContent(string id)
            {
                var body = new
                {
                    accountGroupId = id
                };
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }

            public string GetResult_SimpleAPICall(string id)
            {
                HttpRequestMessage requestMessage = RequestMessages.requestMessage(_method, _HttpContent(id), _uri);
                Console.WriteLine($"Getting Members for account group: {id}");
                return API.API.GetResult(requestMessage, _token);
            }

            internal List<int> _AccountMemberIdList_FromResponse(string response)
            {
                List<int> members = new List<int>();
                JObject jo = JObject.Parse(response);
                int length = 0;
                try //try to find how many groups. unknown length. must be a better solution.
                {
                    while (jo["accountIds"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                //parse id out and put them into the list
                for (int i = 0; i < length; i++)
                {
                    int memberId = Int32.Parse(jo["accountIds"][i].ToString());
                    members.Add(memberId);
                }

                return members;
            }

            internal List<int> _AccountMemberIdList_FromAccountGroupId(string id)
            {
                string resp = GetResult_SimpleAPICall(id);
                return _AccountMemberIdList_FromResponse(resp);
            }

            
        }
        public class GetAccountGroupDefinitions
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private string _token;
            private TaskConfiguration _c;

            //ctor
            public GetAccountGroupDefinitions(TaskConfiguration config)
            {
                _c = config;
                _ServiceInit(_c);
            }

            private void _ServiceInit(TaskConfiguration config)
            {
                if (config.env == Config.Production)
                {
                    _token = Data.Clients.prodOAuthClient.token;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.Production.Asterion.AccountGroupService.getCategoryDefinitions;
                    }
                }
                if (config.env == Config.QA)
                {
                    _token = Data.Clients.qaOAuthClient.token;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Asterion.AccountGroupService.getCategoryDefinitions;
                    }
                }

            }

            private HttpContent _HttpContent()
            {
                var body = new { };
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }
            
            public string GetResult_SimpleAPICall()
            {
                HttpRequestMessage requestMessage = RequestMessages.requestMessage(_method, _HttpContent(), _uri);
                Console.WriteLine("Getting Account Group Definition...");
                return API.API.GetResult(requestMessage, _token);
            }

            public List<KeyValuePair<string, string>> ParseOutDefinitionList(string response)
            {
                List<KeyValuePair<string, string>> res = new List<KeyValuePair<string, string>>();
                JObject jo = JObject.Parse(response);
                int length = 0;
                try
                {
                    while (jo["categories"][length] != null)
                    {
                        length++;
                    }
                }
                catch (ArgumentOutOfRangeException) { }

                for (int i = 0; i < length; i++)
                {
                    string categoryId = jo["categories"][i]["categoryId"].ToString();
                    string categoryName = jo["categories"][i]["name"].ToString();
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(categoryId, categoryName);
                    res.Add(pair);
                }
                return res;
            }

            public string GetDefinition_IntegratedAPICall()
            {
                string response = GetResult_SimpleAPICall();
                List<KeyValuePair<string, string>> list = ParseOutDefinitionList(response);
                StringBuilder sb = new StringBuilder();
                foreach(KeyValuePair<string, string> pair in list)
                {
                    sb.AppendLine($"{pair.Key}\t\t\t{pair.Value}");
                }
                return sb.ToString();
            }
        }
        

        public class Utility
        {
            private static DataTable _AccountGroupObjectToDataTable(List<AccountGroup> accountGroupObjList)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(" ");
                dt.Columns.Add("  ");
                dt.Columns.Add("   ");

                foreach (AccountGroup ag in accountGroupObjList)
                {
                    dt.Rows.Add($"accountGroupId: {ag.id}");
                    dt.Rows.Add(ag.name);
                    string[] fields = new string[] { "firstName", "lastName", "email" };
                    dt.Rows.Add("");
                    dt.Rows.Add("Owners:");
                    dt.Rows.Add(fields);

                    //owners
                    foreach (BnetAccount acc in ag.bnetAccountOwners_obj)
                    {
                        dt.Rows.Add(new string[] { acc.firstName, acc.lastName, acc.email });
                    }
                    dt.Rows.Add("");
                    dt.Rows.Add("Members:");
                    dt.Rows.Add(fields);

                    //member
                    foreach (BnetAccount acc in ag.bnetAccountOwners_obj)
                    {
                        dt.Rows.Add(new string[] { acc.firstName, acc.lastName, acc.email });
                    }

                    dt.Rows.Add("");
                    dt.Rows.Add("Scopes:");
                    foreach (string scope in ag.scopes)
                    {
                        dt.Rows.Add(scope);
                    }
                    dt.Rows.Add("");
                    dt.Rows.Add("");
                }

                return dt;
            }
            private static void _WriteToCSVFromDataTable(DataTable dt, string outputFileName)
            {
                FileManager.WriteToCSV_FromDataTable(dt, outputFileName);
            }
            internal static void WriteToCSVFromAccountGroupObjList(List<AccountGroup> accountGroupObjList, string outputFileName)
            {
                DataTable dt = _AccountGroupObjectToDataTable(accountGroupObjList);
                _WriteToCSVFromDataTable(dt, outputFileName);
            }
            
        }
    }
}
