using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.IO
{
    public static class Paths
    {
        //endpoints file loads path and files
        private static string _defaultEndpointPath = "Configuration Files/End Points/";
        private static string _defaltEndpointFileName = "defaultEndPoints.JSON";
        public static string defaultEndpoints = _defaultEndpointPath + _defaltEndpointFileName;

        //OAuth client file loads path and files
        private static string _defaultOAuthClientPath = "Configuration Files/OAuth Clients/";
        private static string _defaultClientFileName = "defaultClients.JSON";
        public static string defaultClients = _defaultOAuthClientPath + _defaultClientFileName;

        //input data path
        public static string inputFilePath = "input/";

        //output data path
        public static string outputFilePath = "output/";

        //Account Group Ids file
        public static string accountGroupIds = "input/AccountGroupId/";
    }
}
