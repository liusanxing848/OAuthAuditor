using AccessAdminAuditorV3.App;
using AccessAdminAuditorV3.App.IO;
using AccessAdminAuditorV3.Core;
using System;
using System.Collections.Generic;
using static AccessAdminAuditorV3.App.TaskConfiguration;

namespace AccessAdminAuditorV3
{
    class Program
    {
        static void Main(string[] args)
        {
            //TaskConfiguration c = new TaskConfiguration();
            //c.env = Config.Production;
            //c.endpoint = Config.Asterion;
            //Tasks.Initialize();
            //string resp = Tasks.AccountGroupServices.GetAccountGroupInfo_SingleAPICall(c, "075C8E4D-E529-4720-9B6B-A061E2B92E4C");
            //string resp = Tasks.AuthorizationServices.GetScopeForRole_SingleAPICall(c, "075C8E4D-E529-4720-9B6B-A061E2B92E4C");
            //Console.WriteLine(resp);
            //string inputFilePath = Paths.inputFilePath += "COD Group Audit for Ryan Ford.csv";
            //string outputFileName = "V3GroupAuditingAccountGroup";
            //Tasks.AccountGroupServices.ExportFullAccountGroupDetailstoCSV_FromReadingFile(c, inputFilePath, outputFileName);
            //Tasks.AuthorizationServices.GetOAuthClientDetailFromClientId(c, "2f02217feef144ed94daae9fcacaea93");
            //Tasks.AuthorizationServices.WhoIsUsingMyScope(c, "account.full", "account_full_withKhoa");

            
            App.UI.MainMenu.Run();

            

        }
    }
}
