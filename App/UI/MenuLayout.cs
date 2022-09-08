using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App.UI
{
    class MenuLayout
    {
        private string _prompt = "";
        private readonly string[] _options;
        private int _currentSelection;
        private int _drawMenuColumnPos;
        private int _drawMenuRowPos;
        private int _menuMaximumWidth;

        public MenuLayout(string[] options, string prompt)
        {
            _prompt = prompt;
            _options = options;
            _currentSelection = 0;


        }

        public int GetMaximumWidth()
        {
            return _menuMaximumWidth;
        }

        public void CenterMenuToConsole()
        {
            _drawMenuColumnPos = GetConsoleWindowWidth() / 2 - (_menuMaximumWidth / 2);
        }

        // Modify the menu to be left justified
        public void ModifyMenuLeftJustified()
        {
            int maximumWidth = 0;
            string space = "";

            foreach (var t in _options)
            {
                if (t.Length > maximumWidth)
                {
                    maximumWidth = t.Length;
                }
            }

            maximumWidth += 6;

            for (int i = 0; i < _options.Length; i++)
            {
                int spacesToAdd = maximumWidth - _options[i].Length;
                for (int j = 0; j < spacesToAdd; j++)
                {
                    space += " ";
                }
                _options[i] = _options[i] + space;
                space = "";
            }

            _menuMaximumWidth = maximumWidth;
        }

        // Modify the menu to be centered in its column
        public void ModifyMenuCentered()
        {
            int maximumWidth = 0;
            string space = "";

            foreach (var t in _options)
            {
                if (t.Length > maximumWidth)
                {
                    maximumWidth = t.Length;
                }
            }

            maximumWidth += 96;     // make widest measurement wider by 10
                                    // modify this number to make menu wider / narrower

            for (int i = 0; i < _options.Length; i++)
            {
                if (_options[i].Length % 2 != 0)
                {
                    _options[i] += " ";     // make all menu items even num char wide
                }

                var minimumWidth = maximumWidth - _options[i].Length;
                minimumWidth /= 2;
                for (int j = 0; j < minimumWidth; j++)
                {
                    space += " ";
                }

                _options[i] = space + _options[i] + space;      // add spaces on either side of each    
                space = "";                             // menu item
            }

            for (int i = 0; i < _options.Length; i++)
            {
                if (_options[i].Length < maximumWidth)      // if any menu item isn't as wide as
                                                            // the max width, add 1 space
                {
                    _options[i] += " ";
                }
            }

            _menuMaximumWidth = maximumWidth;           // set the max width for use later

        }

        // UTILITIES FOR THE CLASS
        public void SetConsoleWindowSize(int width, int height)
        {
            Console.WindowWidth = width;
            Console.WindowHeight = height;
        }

        public static int GetConsoleWindowWidth()
        {
            return Console.WindowWidth;
        }

        public void SetConsoleTextColor(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        public void ResetCursorVisible()
        {
            Console.CursorVisible = Console.CursorVisible != true;
        }

        public void SetCursorPosition(int row, int column)
        {
            if (row > 0 && row < Console.WindowHeight)
            {
                Console.CursorTop = row;
            }

            if (column > 0 && column < Console.WindowWidth)
            {
                Console.CursorLeft = column;
            }
        }

        private int GetStringLines(string input)
        {
            return input.Split('\n').Length;
        }

        // Engine to run the menu and relevant methods
        public int RunMenu()
        {
            bool run = true;
            ConsoleKey keyPressed;
            Console.WriteLine(_prompt);
            _drawMenuRowPos = GetStringLines(_prompt) + 5;
            _drawMenuColumnPos = 1;
            DrawMenu();
            while (run)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
                if (keyPressed == ConsoleKey.UpArrow)   // up arrow
                {
                    _currentSelection--;
                    if (_currentSelection < 1)
                    {
                        _currentSelection = _options.Length;
                    }

                }
                else if (keyPressed == ConsoleKey.DownArrow)  // down arrow
                {
                    _currentSelection++;
                    if (_currentSelection > _options.Length)
                    {
                        _currentSelection = 1;
                    }

                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    run = false;
                }

                // add more key options here with more else if statements
                // just make sure to add the case to your main switch statement

                DrawMenu();
            }


            return _currentSelection;
        }


        private void DrawMenu()
        {
            for (int i = 0; i < _options.Length; i++)
            {
                SetCursorPosition(_drawMenuRowPos + i, _drawMenuColumnPos);
                SetConsoleTextColor(ConsoleColor.White, ConsoleColor.Black);
                if (i == _currentSelection - 1)
                {
                    SetConsoleTextColor(ConsoleColor.Yellow, ConsoleColor.Green);
                }
                Console.WriteLine(_options[i]);
                Console.ResetColor();
            }
        }


    }
}
