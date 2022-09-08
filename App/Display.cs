using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App
{
    public class Display
    {
        public static void DisplayRed(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public static void DisplayGreen(string str)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(str);
            Console.ResetColor();
        }
        public static void DisplayYellow(string str)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(str);
            Console.ResetColor();
        }
    }
}
