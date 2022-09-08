using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.UtilityPages
{
    class UserInputPage
    {
        private static string[] _option;
        public static void Run(string prompt, out string userInput, Action pageOption)
        {
            Console.WriteLine(prompt);
            Console.WriteLine();
            userInput = Console.ReadLine();
            pageOption.Invoke();
        }
    }
}
