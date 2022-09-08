using AccessAdminAuditorV3.App.UI.UtilityPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.SingleCall.BnetAccountService
{
    class BattlenetAccountServiceHomePage
    {
        private static string[] _options = new string[]
        {
            "Get Battle.net Account Info",
            "Back"
        };

        private static string _prompt = "[Battle.net Account Service HomePage]\nPlease select service";

        public static void Run()
        {
            MenuLayout menu = new MenuLayout(_options, _prompt);
            int curr = menu.RunMenu();

            switch(curr)
            {
                case 0:
                    Console.Clear();
                    BattlenetAccountServiceHomePage.Run();
                    break;
                case 1:
                    _GetBnetAccountInfo();
                    break;
                case 2:
                    _Back();
                    break;
                default:
                    break;
            }

        }

        private static void _GetBnetAccountInfo()
        {
            Console.Clear();
            string rawId = "";
            string prompt = "Plese enter Battle.net account ID";
            UserInputPage.Run(prompt, out rawId, Console.Clear);
            Tasks.BattlenetAccountServices.GetAccountInfo_DisplayIntegratedObjInfo(TaskConfiguration.c, Int32.Parse(rawId));
            DisplayLogPage.Run(BattlenetAccountServiceHomePage.Run);
        }

        private static void _Back()
        {
            Console.Clear();
            SingleCallHomePage.Run();
        }
    }
}
