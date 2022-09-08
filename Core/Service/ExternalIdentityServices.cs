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
using static AccessAdminAuditorV3.Core.Service.BattlenetAccountServices;

namespace AccessAdminAuditorV3.Core.Service
{
    public class ExternalIdentityServices
    {
        public class GetAccountIdByExternalIdentity
        {
            private string _uri;
            private HttpMethod _method = HttpMethod.Post;
            private string _token;
            private TaskConfiguration _c;

            public GetAccountIdByExternalIdentity(TaskConfiguration config)
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
                        _uri = Data.Endpoints.Production.Asterion.ExternalIdentityService.getAccountIdByExternalIdentiy;
                    }
                }
                if (config.env == Config.QA)
                {
                    _token = Data.Clients.qaOAuthClient.token;
                    if (config.endpoint == Config.Asterion)
                    {
                        _uri = Data.Endpoints.QA.Asterion.ExternalIdentityService.getAccountIdByExternalIdentiy;
                    }
                }

            }

            private HttpContent _HttpContent (string email)
            {
                var body = new
                {
                    externalIdentityDomain = 1,
                    externalIdentityEnvironment = 1,
                    externalIdentityUniqueId = email
                };
                string serializedJsonBody = JsonConvert.SerializeObject(body);
                return new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            }

            //[API] single call
            public string GetResult_SimpleAPICall(string email)
            {
                HttpRequestMessage requestMessage = RequestMessages.requestMessage(_method, _HttpContent(email), _uri);
                Console.WriteLine($"Getting account ID for email: {email}");
                return API.API.GetResult(requestMessage, _token);
            }

            //[API] single Call
            public BnetAccount getBnetAccountObjFromEmail(string email)
            {
                string extIdentityResp = GetResult_SimpleAPICall(email);
                int bnetAccountId = _GetAccountIdfromResponse(extIdentityResp);
                BnetAccount acc = new GetAccountInfo(_c).CreateBnetAccountObjFromAccountId(bnetAccountId);
                GetAccountInfo.DisplayBnetAccountObj(acc);
                return acc;
            }

            //[API] Batch Call, multi-threading
            internal List<string> _GetResult_BatchCall(List<string> emailList)
            {
                List<string> resp = new List<string>();
                Parallel.ForEach(emailList, email =>
                {
                    resp.Add(GetResult_SimpleAPICall(email));
                });
                return resp;
            }

            //[Parse]
            internal int _GetAccountIdfromResponse(string response)
            {
                JObject jo = JObject.Parse(response);
                try
                {
                    int id = Int32.Parse(jo["accountId"].ToString());
                    return id;
                }
                catch (Exception e)
                {
                    Error.sb.Append($"Error at parsing account id from Ext Identity service {e}");
                    return -1;
                }
            }
        }
    }
}
