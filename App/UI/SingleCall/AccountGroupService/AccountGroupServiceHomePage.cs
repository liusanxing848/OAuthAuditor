using AccessAdminAuditorV3.App.UI.UtilityPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.SingleCall.AccountGroupService
{
    class AccountGroupServiceHomePage
    {
        private static string[] _options = new string[]
        {
            "Get Account Group Detail",
            "Get Account Group Definition Category",
            "Back"
        };

        private static string _prompt = "[Account Group Service Home Page]\nPlease select Account Group Service";

        public static void Run()
        {
            MenuLayout menu = new MenuLayout(_options, _prompt);
            int curr = menu.RunMenu();

            switch(curr)
            {
                case 0:
                    Console.Clear();
                    AccountGroupServiceHomePage.Run();
                    break;
                case 1: //Get Account Group Detail
                    _GetAccountGroupDetail();
                    break;
                case 2:
                    _GetAccountGroupDefinition();
                    break;
                case 3:
                    Console.Clear();
                    SingleCallHomePage.Run();
                    break;
                default:
                    break;
            }

            
        }

        private static void _GetAccountGroupDetail()
        {
            Console.Clear();
            string id = "";
            string prompt = "Please enter Account Group ID";
            UserInputPage.Run(prompt, out id, Console.Clear);
            Tasks.AccountGroupServices.GetFullAccountGroupInfo_FullObjDisplay(TaskConfiguration.c, id);
            DisplayLogPage.Run(AccountGroupServiceHomePage.Run);
        }

        private static void _GetAccountGroupDefinition()
        {
            Console.Clear();
            string display = Tasks.AccountGroupServices.GetAccountGroupDefinition_Integrated(TaskConfiguration.c);
            Console.WriteLine(display);
            DisplayLogPage.Run(AccountGroupServiceHomePage.Run);
        }
    }
}
