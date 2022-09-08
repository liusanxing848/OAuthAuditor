using AccessAdminAuditorV3.App.UI.UtilityPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.BatchCallExport
{
    class BatchCallExportHomePage
    {
        private static string[] _options = new string[]
        {
            "Account Group Auditing (Import account group Id from file)",
            "Who's using my scope?",
            "Back"
        };

        private static string _prompt = "Please select task";

        public static void Run()
        {
            
            MenuLayout menu = new MenuLayout(_options, _prompt);
            int curr = menu.RunMenu();

            switch (curr)
            {
                case 0:
                    Console.Clear();
                    BatchCallExportHomePage.Run();
                    break;
                case 1: // Account Group Auditing
                    _AccountGroupAuditing();
                    break;
                case 2: //Who's using my scope
                    _WhosUsingMyScope();
                    break;
                case 3: //Back
                    _Back();
                    break;
                default:
                    break;
            }
        }


        private static void _Back()
        {
            Console.Clear();
            MainMenu.Run();
        }
        private static void _AccountGroupAuditing()
        {
            Console.Clear();
            //1.get the file Path
            string inputFilePath = "";

            //2. get The output file name;
            string outputFileName = "";
            string userInputPagePrompt = "Please name your file, this is already csv type, DO NOT add file type";
            SelectInputFile.Run(out inputFilePath, Console.Clear, BatchCallExportHomePage.Run);
            UserInputPage.Run(userInputPagePrompt, out outputFileName, Console.Clear);
            Tasks.AccountGroupServices.ExportFullAccountGroupDetailstoCSV_FromReadingFile(TaskConfiguration.c, inputFilePath, outputFileName);  
            DisplayLogPage.Run(BatchCallExportHomePage.Run);
            
        }

        private static void _WhosUsingMyScope()
        {
            Console.Clear();

            //1.Get the scope
            string scope = "";
            //2.Get the outputfilename;
            string fileName = "";
            string promptScopeInput = "Please enter the scope name you feel butt hurt about";
            string promptOutputFileName = "Please name your file DO NOT ADD FILE TYPE";
            UserInputPage.Run(promptScopeInput, out scope, Console.Clear);
            UserInputPage.Run(promptOutputFileName, out fileName, Console.Clear);
            Tasks.AuthorizationServices.WhoIsUsingMyScope(TaskConfiguration.c, scope, fileName);
            DisplayLogPage.Run(BatchCallExportHomePage.Run);
        }
    }
}
