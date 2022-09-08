using AccessAdminAuditorV3.App;
using AccessAdminAuditorV3.App.Data;
using AccessAdminAuditorV3.Core.API;
using AccessAdminAuditorV3.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static AccessAdminAuditorV3.App.TaskConfiguration;

namespace AccessAdminAuditorV3.Core.Service
{
    public class BattlenetAccountServices
    {

        public class GetAccountInfo
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private string _token;
            private TaskConfiguration _c;

            public GetAccountInfo(TaskConfiguration config)
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
                        _uri = Data.Endpoints.Production.Asterion.BattleNetAccountService.getAccountInfo;
                    }
                }
                if (config.env == Config.QA)
                {
                    _token = Data.Clients.qaOAuthClient.token;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Asterion.BattleNetAccountService.getAccountInfo;
                    }
                }
            }

            private HttpContent _HttpContent(int id)
            {
                var body = new
                {
                    accountId = id
                };
                
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }


            //[API] single call
            public string GetResult_SimpleAPICall(int id)
            {
                if(id == -1)
                {
                    return "-1";
                }
                HttpRequestMessage requestMessage = RequestMessages.requestMessage(_method, _HttpContent(id), _uri);
                Console.WriteLine($"Getting Battle.net accout information for {id}");
                return API.API.GetResult(requestMessage, _token);
            }

            //[API] Batch, threaded
            public List<string> GetResult_BatchCall(List<int> BnetAccountIds)
            {
                List<string> resp = new List<string>();
                Parallel.ForEach(BnetAccountIds, id =>
                {
                    resp.Add(GetResult_SimpleAPICall(id)); 
                });
                return resp;
            }

            //[Parse] [Obj]
            public BnetAccount CreateBnetAccountObjFromResponse(string resp)
            {
                if(resp == "-1")
                {
                    BnetAccount badAccount = new BnetAccount();
                    badAccount.id = -1;
                    badAccount.lastName = "N/A";
                    badAccount.firstName = "N/A";
                    badAccount.battleTag = "N/A";
                    badAccount.email = "N/A";
                    return badAccount;
                }
                BnetAccount acc = new BnetAccount();
                JObject jo = JObject.Parse(resp);
                try
                {
                    acc.id = Int32.Parse(jo["accountInfo"]["accountId"].ToString());
                    acc.firstName = jo["accountInfo"]["firstName"].ToString();
                    acc.lastName = jo["accountInfo"]["lastName"].ToString();
                    acc.email = jo["accountInfo"]["email"].ToString();
                    acc.battleTag = jo["accountInfo"]["battleTag"].ToString();
                }
                catch (Exception e)
                {
                    Display.DisplayRed($"Error while parsing Battle.net Account: {resp}");
                    Error.sb.Append($"Error {e} while parsing Battle.net Account: {resp} \n\n");
                }
                return acc;
            }

            public List<BnetAccount> CreateBnetAccountObjListFromResponse_Batch(List<string> responses)
            {
                List<BnetAccount> list = new List<BnetAccount>();
                foreach (string resp in responses)
                {
                    list.Add(CreateBnetAccountObjFromResponse(resp));
                }
                return list;
            }

            public BnetAccount CreateBnetAccountObjFromAccountId(int id)
            {
                return CreateBnetAccountObjFromResponse(GetResult_SimpleAPICall(id));
            }

            public List<BnetAccount> CreateBnetAccountObjListFromAccountId_Batch(List<int> ids)
            {              
                return CreateBnetAccountObjListFromResponse_Batch(GetResult_BatchCall(ids));
            }

            public static void DisplayBnetAccountObj(BnetAccount acc)
            {
                Display.DisplayGreen("___________________________________________________");
                Console.Write("Battle.net account id: ");
                Display.DisplayYellow(acc.id.ToString());
                Console.Write("Name: ");
                Display.DisplayYellow($"{acc.firstName} {acc.lastName}");
                Console.Write("Email: ");
                Display.DisplayYellow(acc.email);
                Console.Write("BattleTag: ");
                Display.DisplayYellow(acc.battleTag);
            }
            public void DisplayBnetAccountObj2(BnetAccount acc)
            {
                Display.DisplayGreen("___________________________________________________");
                Console.Write("Battle.net account id: ");
                Display.DisplayYellow(acc.id.ToString());
                Console.Write("Name: ");
                Display.DisplayYellow($"{acc.firstName} {acc.lastName}");
                Console.Write("Email: ");
                Display.DisplayYellow(acc.email);
                Console.Write("BattleTag: ");
                Display.DisplayYellow(acc.battleTag);
            }

            public void DisplayBnetInfoFromId(int id)
            {
                BnetAccount acc = CreateBnetAccountObjFromAccountId(id);
                DisplayBnetAccountObj2(acc);
            }
        }
        
    }
}
