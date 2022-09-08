using AccessAdminAuditorV3.App.UI.BatchCallExport;
using AccessAdminAuditorV3.App.UI.SingleCall;
using AccessAdminAuditorV3.App.UI.UtilityPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI
{
    class MainMenu
    {
        public static string[] options = new string[]
        {
            "Initialize",
            "Set Environment & Service Provider Configurations",
            "Single Call",
            "Batch Call & Export",
            "Exit"
        };

        public static void Run()
        {
            MenuLayout page = new MenuLayout(options, _prompt);
            int currentSelection = page.RunMenu();
            switch(currentSelection)
            {
                case 0:
                    Console.Clear();
                    MainMenu.Run();
                    break;
                case 1: //Initialize
                    _Initializie();
                    break;
                case 2: //Set Environment & Service Configurations
                    _Set_Environment_Service_Provider_Configurations();
                    break;
                case 3: //SingleCall
                    _SingleCall();
                    break;
                case 4: //Batch Call & Export
                    _BatchCallExport();
                    break;
                case 5: //Initialize
                    Console.Clear();
                    MainMenu.Run();
                    break;
                default:
                    break;

            }
        }

        private static void _Initializie()
        {
            Console.Clear();
            Tasks.Initialize();
            DisplayLogPage.Run(MainMenu.Run);
        }

        private static void _Set_Environment_Service_Provider_Configurations()
        {
            Console.Clear();
            SetConfigurationPage.Run(MainMenu.Run);
        }

        private static void _BatchCallExport()
        {
            Console.Clear();
            BatchCallExportHomePage.Run();
        }

        private static void _SingleCall()
        {
            Console.Clear();
            SingleCallHomePage.Run();
        }













        private static string _prompt = @"         
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
░░                                                                                                        ░░
░░   ██████   █████  ██    ██ ████████ ██   ██      █████  ██    ██ ██████  ██ ████████  ██████  ██████   ░░
░░  ██    ██ ██   ██ ██    ██    ██    ██   ██     ██   ██ ██    ██ ██   ██ ██    ██    ██    ██ ██   ██  ░░
░░  ██    ██ ███████ ██    ██    ██    ███████     ███████ ██    ██ ██   ██ ██    ██    ██    ██ ██████   ░░
░░  ██    ██ ██   ██ ██    ██    ██    ██   ██     ██   ██ ██    ██ ██   ██ ██    ██    ██    ██ ██   ██  ░░
░░   ██████  ██   ██  ██████     ██    ██   ██     ██   ██  ██████  ██████  ██    ██     ██████  ██   ██  ░░
░░                                                                                                        ░░
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
";
    }
}
