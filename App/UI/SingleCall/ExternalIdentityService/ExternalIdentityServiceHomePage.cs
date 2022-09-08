using AccessAdminAuditorV3.App.UI.UtilityPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.SingleCall.ExternalIdentityService
{
    class ExternalIdentityServiceHomePage
    {
        private static string[] _options = new string[]
        {
            "Get Battle.net account detail from email",
            "Back"
        };

        private static string _prompt = "[External Identity Service Home Page]\n Please select service";

        public static void Run()
        {
            MenuLayout menu = new MenuLayout(_options, _prompt);
            int curr = menu.RunMenu();

            switch(curr)
            {
                case 0:
                    Console.Clear();
                    ExternalIdentityServiceHomePage.Run();
                    break;
                case 1:
                    _GetBnetAccountDetailFromEmail();
                    break;
                case2:
                    _Back();
                default:
                    break;
            }

        }

        private static void _Back()
        {
            Console.Clear();
            SingleCallHomePage.Run();
        }

        private static void _GetBnetAccountDetailFromEmail()
        {
            Console.Clear();
            string email = "";
            string prompt = "Please enter the email";
            UserInputPage.Run(prompt, out email, Console.Clear);
            Tasks.ExternalIdentiyServices.GetBnetAccountDetailFromEmailDisplay(TaskConfiguration.c, email);
            DisplayLogPage.Run(ExternalIdentityServiceHomePage.Run);
        }
    }
}
