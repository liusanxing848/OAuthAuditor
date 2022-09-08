using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AccessAdminAuditorV3.Core.API
{
    public static class API
    {
        /// <summary>
        /// Send Request and get result
        /// </summary>
        /// <param name="httpRequestMessage">Http Request message contains http methods and request infomation</param>
        /// <param name="token">Optional, if a request needs token as request header</param>
        /// <returns>the result from the request</returns>
        private static string _GetResult(HttpRequestMessage httpRequestMessage, string token = null)
        {
            try
            {
                HttpClient c = new HttpClient();
                c.DefaultRequestHeaders.Clear();

                if(token != null)
                {
                    c.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                return c.SendAsync(httpRequestMessage).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
                return null;
            }
        }

        public static string GetResult(HttpRequestMessage httpReqeustMessage)
        {
            return _GetResult(httpReqeustMessage);
        }

        public static string GetResult(HttpRequestMessage httpRequestMessage, string token)
        {
            return _GetResult(httpRequestMessage, token);
        }
    }
}
