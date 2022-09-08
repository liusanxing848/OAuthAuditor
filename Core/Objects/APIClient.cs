using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.Core.Objects
{
    public class APIClient
    {
        public string name;
        public string id;
        public string secret;
        public string token;
        public List<string> scopes = new List<string>();

        public APIClient(string name)
        {
            this.name = name;
        }
    }
}
