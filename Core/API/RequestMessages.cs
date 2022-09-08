using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AccessAdminAuditorV3.Core.API
{
    class RequestMessages
    {
        
        public static HttpRequestMessage requestMessage(HttpMethod method, HttpContent httpContent, string uri)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, uri);
            message.Content = httpContent;
            return message;
        }

        public static HttpRequestMessage requestMessage_Auth(HttpMethod method, HttpContent httpContent, string clientID, string clientSecret, string uri)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, uri);
            message.Headers.Authorization = _Header(clientID, clientSecret);
            message.Content = httpContent;
            return message;
        }

        private static AuthenticationHeaderValue _Header(string clientID, string clientSecret)
        {
            string value = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(
                                                    ($"{clientID}:{clientSecret}")));

            string key = "Basic";
            KeyValuePair<string, string> header = new KeyValuePair<string, string>(key, value);
            return new AuthenticationHeaderValue(header.Key, header.Value);
        }
    }
}
