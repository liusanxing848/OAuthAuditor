using AccessAdminAuditorV3.App.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.UtilityPages
{
    class SelectInputFile
    {
        private static string[] _options;
        private static string _prompt = "Please select file";

        public static void Run(out string inputFilePath, Action ForwardPageOption = null, Action BackwardPageOption = null)
        {
            List<string> opts = FileManager.FileToMenuOption(Paths.inputFilePath);
            _options = new string[opts.Count];
            _options = opts.ToArray();
            MenuLayout menu = new MenuLayout(_options, _prompt);
            int curr = menu.RunMenu();
            if(curr == 0)
            {
                Console.Clear();
                SelectInputFile.Run(out inputFilePath, ForwardPageOption, BackwardPageOption);
            }
            if(curr == _options.Length)
            {
                Console.Clear();
                BackwardPageOption.Invoke();
                inputFilePath = null;
            }
            else
            {
                Console.Clear();
                inputFilePath = Paths.inputFilePath + _options[curr - 1];
                ForwardPageOption.Invoke();
            }
        }


    }
}
