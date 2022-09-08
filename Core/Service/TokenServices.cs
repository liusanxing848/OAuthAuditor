using AccessAdminAuditorV3.App;
using AccessAdminAuditorV3.Core.API;
using AccessAdminAuditorV3.Core.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace AccessAdminAuditorV3.Core.Service
{
    class TokenServices
    {
        private static HttpContent _content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "client_credentials" },
                    {"include_available_scopes", "true" }
                });
        private static HttpMethod _method = HttpMethod.Post;
        private static void _ValidateClient()
        {
            if (Data.Clients.prodOAuthClient.id == null || Data.Clients.prodOAuthClient.secret == null)
            {
                Display.DisplayRed("Production Client is not loaded!");
            }
            if (Data.Clients.qaOAuthClient.id == null || Data.Clients.qaOAuthClient.secret == null)
            {
                Display.DisplayRed("QA Client is not loaded!");
            }
        }
        private static void _ValidateEndpoints()
        {
            if (Data.Endpoints.QA.TokenService.getToken == null)
            {
                Display.DisplayRed("QA GetToken service endpoint not laoded!");
            }
            if (Data.Endpoints.Production.TokenService.getToken == null)
            {
                Display.DisplayRed("Produciton GetToken service endpoint not loaded!");
            }
        }

        public static void GetAllClientsToken()
        {
            GetQAToken();
            GetProdToken();
        }

        public static void GetQAToken()
        {
            _ValidateClient();
            _ValidateEndpoints();
            string qaClientId = Data.Clients.qaOAuthClient.id;
            string qaSecret = Data.Clients.qaOAuthClient.secret;
            string uri = Data.Endpoints.QA.TokenService.getToken;
            HttpRequestMessage requestMessage = RequestMessages.requestMessage_Auth(_method, _content, qaClientId, qaSecret, uri);
            Console.WriteLine($"Fetching token for QA client {qaClientId}");
            string respond = API.API.GetResult(requestMessage);
            _ParseResponseAndConstructObject(Data.Clients.qaOAuthClient, respond);
            _PrintClientInfo(Data.Clients.qaOAuthClient);
        }
        
        public static void GetProdToken()
        {
            _ValidateClient();
            _ValidateEndpoints();
            string prodClientId = Data.Clients.prodOAuthClient.id;
            string prodSecret = Data.Clients.prodOAuthClient.secret;
            string uri = Data.Endpoints.Production.TokenService.getToken;
            HttpRequestMessage requestMessage = RequestMessages.requestMessage_Auth(_method, _content, prodClientId, prodSecret, uri);
            Console.WriteLine($"Fetching token for Prod client {prodClientId}");
            string respond = API.API.GetResult(requestMessage);
            _ParseResponseAndConstructObject(Data.Clients.prodOAuthClient, respond);
            _PrintClientInfo(Data.Clients.prodOAuthClient);
        }

        private static void _ParseResponseAndConstructObject(APIClient client, string response)
        {
            JObject jo = JObject.Parse(response);
            client.token = jo["access_token"].ToString();
            client.scopes = jo["scope"].ToString().Split(' ').ToList();
        }

        private static void _PrintClientInfo(APIClient client)
        {
            Console.WriteLine($"client name: {client.name}");
            Console.WriteLine($"client ID: {client.id}");
            Console.WriteLine($"client secret: {client.secret}");
            Console.Write("token: ");
            Display.DisplayYellow(client.token);
            Console.WriteLine("scope: ");
            foreach(string scope in client.scopes)
            {
                Display.DisplayYellow(scope);
            }
            Console.WriteLine("\n\n\n");
        }
    }
}
