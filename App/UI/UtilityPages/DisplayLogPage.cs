using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI.UtilityPages
{
    class DisplayLogPage
    {
        private static string[] _options = new string[] {};

        public static void Run(Action pageOption = null)
        {
            MenuLayout menu = new MenuLayout(_options, "");
            int curr = menu.RunMenu();
            switch(curr)
            {
                case 0:
                    Console.Clear();
                    pageOption.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}
