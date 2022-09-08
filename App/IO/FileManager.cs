using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AccessAdminAuditorV3.App.IO
{
    class FileManager
    {
        public static string ReadFileAsString(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public static List<string> ReadCSVtoList(string filePath)
        {
            List<string> list = new List<string>();
            StreamReader reader = new StreamReader(filePath);

            string line = "";

            while((line = reader.ReadLine()) != null)
            {
                list.Add(line);
            }

            return list;
        }

        public static void WriteToCSV_FromString(string finalContent, string fileName)
        {
            StreamWriter writer = new StreamWriter(Paths.outputFilePath + fileName + ".csv");
            writer.WriteLine(finalContent);
            writer.Close();
        }

        public static void WriteToCSV_FromDataTable(DataTable dt, string fileName)
        {
            WriteToCSV_FromString(_DataTableToString(dt), fileName);
        }

        private static string _DataTableToString(DataTable dT)
        {
            StringBuilder sb = new StringBuilder();
            //take column name

            for (int col = 0; col < dT.Columns.Count; col++)
            {
                //the last col name should not end with ,
                if (col == dT.Columns.Count - 1)//means the last element
                {
                    sb.Append(dT.Columns[col].ColumnName.ToString().Replace(',', ';')); //originaltest should not include ','

                }
                else
                {
                    sb.Append(dT.Columns[col].ColumnName.ToString().Replace(',', ';') + ',');
                }
            }

            sb.Append(Environment.NewLine);
            for (int ro = 0; ro < dT.Rows.Count; ro++)
            {
                for (int col = 0; col < dT.Columns.Count; col++)
                {
                    if (col == dT.Columns.Count - 1)
                    {
                        sb.Append(dT.Rows[ro][col].ToString().Replace(',', ';'));
                    }
                    else
                    {
                        sb.Append(dT.Rows[ro][col].ToString().Replace(',', ';') + ',');
                    }
                }

                if (ro != dT.Rows.Count - 1)
                {
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        public static void WriteToTXT(string finalContent, string fileName)
        {
            StreamWriter writer = new StreamWriter(Paths.outputFilePath + fileName + ".txt");
            writer.WriteLine(finalContent);
            writer.Close();
        }

        public static void LoadEndpoints(string filePath)
        {
            string fileContent = FileManager.ReadFileAsString(filePath);
            JObject jo = JObject.Parse(fileContent);

            //tokens
            
            Core.Data.Endpoints.QA.TokenService.getToken = jo["QA"]["token"].ToString();
            Core.Data.Endpoints.Production.TokenService.getToken = jo["Production"]["token"].ToString();

            //Asterion baseURI - QA & Prod
            Core.Data.Endpoints.QA.Asterion.baseURI = jo["QA"]["Asterion"].ToString();
            Core.Data.Endpoints.Production.Asterion.baseURI = jo["Production"]["Asterion"].ToString();

            //Auth baseURI - QA & Prod
            Core.Data.Endpoints.QA.Auth.baseURI = jo["QA"]["Auth"].ToString();
            Core.Data.Endpoints.Production.Auth.baseURI = jo["Production"]["Auth"].ToString();

            //Asterion Battlenet account service - baseURI & end points QA Prod
            Core.Data.Endpoints.QA.Asterion.BattleNetAccountService.baseURI = jo["BattleNetAccountService"]["baseURI"].ToString();
            Core.Data.Endpoints.QA.Asterion.BattleNetAccountService.getAccountInfo = Core.Data.Endpoints.QA.Asterion.baseURI + Core.Data.Endpoints.QA.Asterion.BattleNetAccountService.baseURI + jo["BattleNetAccountService"]["getAccountInfo"];
            Core.Data.Endpoints.Production.Asterion.BattleNetAccountService.baseURI = jo["BattleNetAccountService"]["baseURI"].ToString();
            Core.Data.Endpoints.Production.Asterion.BattleNetAccountService.getAccountInfo = Core.Data.Endpoints.Production.Asterion.baseURI + Core.Data.Endpoints.Production.Asterion.BattleNetAccountService.baseURI + jo["BattleNetAccountService"]["getAccountInfo"];

            //Asterion AccountGroup Service - base URI & end point,  QA & Prod
            Core.Data.Endpoints.QA.Asterion.AccountGroupService.baseURI = jo["AccountGroupService"]["baseURI"].ToString();
            Core.Data.Endpoints.QA.Asterion.AccountGroupService.getCategoryDefinitions = Core.Data.Endpoints.QA.Asterion.baseURI + Core.Data.Endpoints.QA.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getCategoryDefinitions"].ToString();
            Core.Data.Endpoints.QA.Asterion.AccountGroupService.getAccountGroupsByCategory = Core.Data.Endpoints.QA.Asterion.baseURI + Core.Data.Endpoints.QA.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getAccountGroupsByCategory"].ToString();
            Core.Data.Endpoints.QA.Asterion.AccountGroupService.getAccountGroup = Core.Data.Endpoints.QA.Asterion.baseURI + Core.Data.Endpoints.QA.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getAccountGroup"].ToString();
            Core.Data.Endpoints.QA.Asterion.AccountGroupService.getMembersForGroup = Core.Data.Endpoints.QA.Asterion.baseURI + Core.Data.Endpoints.QA.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getMembersFromAccountGroup"].ToString();
            
            Core.Data.Endpoints.Production.Asterion.AccountGroupService.baseURI = jo["AccountGroupService"]["baseURI"].ToString();
            Core.Data.Endpoints.Production.Asterion.AccountGroupService.getCategoryDefinitions = Core.Data.Endpoints.Production.Asterion.baseURI + Core.Data.Endpoints.Production.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getCategoryDefinitions"].ToString();
            Core.Data.Endpoints.Production.Asterion.AccountGroupService.getAccountGroupsByCategory = Core.Data.Endpoints.Production.Asterion.baseURI + Core.Data.Endpoints.Production.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getAccountGroupsByCategory"].ToString();
            Core.Data.Endpoints.Production.Asterion.AccountGroupService.getAccountGroup = Core.Data.Endpoints.Production.Asterion.baseURI + Core.Data.Endpoints.Production.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getAccountGroup"].ToString();
            Core.Data.Endpoints.Production.Asterion.AccountGroupService.getMembersForGroup = Core.Data.Endpoints.Production.Asterion.baseURI + Core.Data.Endpoints.Production.Asterion.AccountGroupService.baseURI + jo["AccountGroupService"]["getMembersFromAccountGroup"].ToString();

            //Asterion External Identity Service - baseURI & end points QA Prod
            Core.Data.Endpoints.QA.Asterion.ExternalIdentityService.baseURI = jo["ExternalIdentityService"]["baseURI"].ToString();
            Core.Data.Endpoints.QA.Asterion.ExternalIdentityService.getAccountIdByExternalIdentiy = Core.Data.Endpoints.QA.Asterion.baseURI + Core.Data.Endpoints.QA.Asterion.ExternalIdentityService.baseURI + jo["ExternalIdentityService"]["getAccountIdByEmail"];
            Core.Data.Endpoints.Production.Asterion.ExternalIdentityService.baseURI = jo["ExternalIdentityService"]["baseURI"].ToString();
            Core.Data.Endpoints.Production.Asterion.ExternalIdentityService.getAccountIdByExternalIdentiy = Core.Data.Endpoints.Production.Asterion.baseURI + Core.Data.Endpoints.Production.Asterion.ExternalIdentityService.baseURI + jo["ExternalIdentityService"]["getAccountIdByEmail"];

            //Auth Authorization Service - baseURI & end points QA Prod
            Core.Data.Endpoints.QA.Auth.AuthorizationService.baseURI = jo["AuthorizationService"]["baseURI"].ToString();
            Core.Data.Endpoints.QA.Auth.AuthorizationService.getScope = Core.Data.Endpoints.QA.Auth.baseURI + Core.Data.Endpoints.QA.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getScopeInfo"].ToString();
            Core.Data.Endpoints.QA.Auth.AuthorizationService.getClientIdByScope = Core.Data.Endpoints.QA.Auth.baseURI + Core.Data.Endpoints.QA.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getClientByScope"].ToString();
            Core.Data.Endpoints.QA.Auth.AuthorizationService.getClient = Core.Data.Endpoints.QA.Auth.baseURI + Core.Data.Endpoints.QA.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getClientInfo"].ToString();
            Core.Data.Endpoints.QA.Auth.AuthorizationService.getScopesForRole = Core.Data.Endpoints.QA.Auth.baseURI + Core.Data.Endpoints.QA.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getScopesForRole"].ToString();


            Core.Data.Endpoints.Production.Auth.AuthorizationService.baseURI = jo["AuthorizationService"]["baseURI"].ToString();
            Core.Data.Endpoints.Production.Auth.AuthorizationService.getScope = Core.Data.Endpoints.Production.Auth.baseURI + Core.Data.Endpoints.Production.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getScopeInfo"].ToString();
            Core.Data.Endpoints.Production.Auth.AuthorizationService.getClientIdByScope = Core.Data.Endpoints.Production.Auth.baseURI + Core.Data.Endpoints.Production.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getClientByScope"].ToString();
            Core.Data.Endpoints.Production.Auth.AuthorizationService.getClient = Core.Data.Endpoints.Production.Auth.baseURI + Core.Data.Endpoints.Production.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getClientInfo"].ToString();
            Core.Data.Endpoints.Production.Auth.AuthorizationService.getScopesForRole = Core.Data.Endpoints.Production.Auth.baseURI + Core.Data.Endpoints.Production.Auth.AuthorizationService.baseURI + jo["AuthorizationService"]["getScopesForRole"].ToString();
        }
        public static void LoadOAuthClients(string filePath)
        {
            string fileContent = FileManager.ReadFileAsString(filePath);
            JObject jo = JObject.Parse(fileContent);
            

            string qaID = jo["QA"]["ID"].ToString();

            string qaSecret = jo["QA"]["secret"].ToString();

            string prodID = jo["Production"]["ID"].ToString();

            string prodSecret = jo["Production"]["secret"].ToString();



            Core.Data.Clients.qaOAuthClient.id = qaID;
            Core.Data.Clients.qaOAuthClient.secret = qaSecret;
            Core.Data.Clients.prodOAuthClient.id = prodID;
            Core.Data.Clients.prodOAuthClient.secret = prodSecret;
        }
        public static List<string> FileToMenuOption(string filePath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            FileInfo[] fileInfos = dirInfo.GetFiles("*");

            List<string> menuOptions = new List<string>();
            foreach (FileInfo fInfo in fileInfos)
            {
                menuOptions.Add(fInfo.Name);
            }
            menuOptions.Add("BACK");

            return menuOptions;
        }

    }
}
