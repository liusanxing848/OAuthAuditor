using System;
using System.Collections.Generic;
using System.Text;
using static AccessAdminAuditorV3.App.TaskConfiguration;

namespace AccessAdminAuditorV3.App.UI.UtilityPages
{
    class SetConfigurationPage
    {
        private static string[] _options = new string[]
        {
            "[QA] - [Gateway]",
            "[QA] - [Asterion]",
            "[Production] - [Gateway]",
            "[Production] - [Asterion]",
            "Back"
        };

        private static string _prompt = "\n\n\n\nPlease select the environment and service provider.";

        public static void Run(Action pageOption)
        {
            MenuLayout menu = new MenuLayout(_options, _prompt);
            int curr = menu.RunMenu();
            switch(curr)
            {
                case 0:
                    Console.Clear();
                    SetConfigurationPage.Run(pageOption);
                    break;
                case 1: // QA - GW
                    _QAGW(pageOption);
                    break;
                case 2: //QA - Asterion
                    _QAAst(pageOption);
                    break;
                case 3: //Prod - GW
                    _ProdGW(pageOption);
                    break;
                case 4: //Prod - Asterion
                    _ProdAst(pageOption);
                    break;
                case 5: //Back
                    _Back(pageOption);
                    break;
                default:
                    break;
                    
            }
        }

        private static void _QAGW(Action pageOption)
        {
            Console.Clear();
            TaskConfiguration.c.env = Config.QA;
            TaskConfiguration.c.endpoint = Config.Gateway;
            _ConfigConfirmationDisplay();
            SetConfigurationPage.Run(pageOption);
        }
        private static void _QAAst(Action pageOption)
        {
            Console.Clear();
            TaskConfiguration.c.env = Config.QA;
            TaskConfiguration.c.endpoint = Config.Asterion;
            _ConfigConfirmationDisplay();
            SetConfigurationPage.Run(pageOption);
        }
        private static void _ProdGW(Action pageOption)
        {
            Console.Clear();
            TaskConfiguration.c.env = Config.Production;
            TaskConfiguration.c.endpoint = Config.Gateway;
            _ConfigConfirmationDisplay();
            SetConfigurationPage.Run(pageOption);
        }
        private static void _ProdAst(Action pageOption)
        {
            Console.Clear();
            TaskConfiguration.c.env = Config.Production;
            TaskConfiguration.c.endpoint = Config.Asterion;
            _ConfigConfirmationDisplay();
            SetConfigurationPage.Run(pageOption);
        }

        private static void _Back(Action pageOption)
        {
            Console.Clear();
            pageOption.Invoke();
        }

        private static void _ConfigConfirmationDisplay()
        {
            Console.WriteLine("Configuration has been set!");
            Console.Write("Environment: ");
            Display.DisplayGreen(TaskConfiguration.c.env.ToString());
            Console.Write("Service Provider: ");
            Display.DisplayGreen(TaskConfiguration.c.endpoint.ToString());
            Console.WriteLine("\n");
        }
    }
}
