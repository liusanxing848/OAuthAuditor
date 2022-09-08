using AccessAdminAuditorV3.App.UI.SingleCall.AccountGroupService;
using AccessAdminAuditorV3.App.UI.SingleCall.BnetAccountService;
using AccessAdminAuditorV3.App.UI.SingleCall.ExternalIdentityService;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.SingleCall
{
    class SingleCallHomePage
    {
        private static string[] _options = new string[]
        {
            "Account Group Service",
            "Battle.net Account Service",
            "Authorization Service (under construction)" ,
            "External Identity Service",
            "Back"
        };
        private static string _prompt = "[Single Call Home Page]\nPlease select service";

        public static void Run()
        {
            MenuLayout menu = new MenuLayout(_options, _prompt);
            int curr = menu.RunMenu();
            switch(curr)
            {
                case 0:
                    Console.Clear();
                    SingleCallHomePage.Run();
                    break;
                case 1: //Account Group Service
                    _AccountGroupService();
                    break;
                case 2: // Battlenet Account Service
                    _BattlenetAccountService();
                    break;
                case 3: //Authorization Service
                    Console.Clear();
                    SingleCallHomePage.Run();
                    break;
                case 4: // External Identity Service
                    _ExternalIdentityService();
                    break;
                case 5: // Back
                    _Back();
                    break;
                default:
                    break;

            }
        }

        private static void _AccountGroupService()
        {
            Console.Clear();
            AccountGroupServiceHomePage.Run();
        }
        private static void _BattlenetAccountService()
        {
            Console.Clear();
            BattlenetAccountServiceHomePage.Run();
        }
        private static void _AuthorizationService(Action pageOption)
        {

        }
        private static void _ExternalIdentityService()
        {
            Console.Clear();
            ExternalIdentityServiceHomePage.Run();
        }
        private static void _Back()
        {
            Console.Clear();
            MainMenu.Run();
        }
    }
}
