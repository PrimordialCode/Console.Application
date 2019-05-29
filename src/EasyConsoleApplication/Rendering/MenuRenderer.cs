﻿using System;

namespace EasyConsoleApplication.Menus
{
    internal class Rendering
    {
        /// <summary>
        /// variable that will be chacked to know if it's time
        /// to terminate the loop that keep asking for new commands
        /// from this menu
        /// </summary>
        internal bool terminate;

        /// <summary>
        /// Call exit whenever you want to terminate the loop that
        /// keep asking for new commands from this menu
        /// </summary>
        public void Exit()
        {
            terminate = true;
        }

        public void Render(
            string title,
            string body,
            Menu menu)
        {
            var menuItems = menu.Items;
            while (true)
            {
                Console.Clear();
                if (!string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine(title);
                }
                if (!string.IsNullOrWhiteSpace(body))
                {
                    Console.WriteLine(body);
                }
                if (menuItems != null)
                {
                    for (int idx = 0; idx < menuItems.Count; idx++)
                    {
                        var mi = menuItems[idx];
                        var cmd = mi.Command;
                        if (string.IsNullOrWhiteSpace(cmd))
                        {
                            cmd = (idx + 1).ToString();
                        }
                        Console.WriteLine($" {cmd} - {mi.Title}");
                    }
                    string value = ConsoleHelpers.Readline(Console.ForegroundColor, "Select an option: ");

                    // check the string commands first
                    var selectedMi = menuItems.Find(m => m.Command == value);
                    if (selectedMi != null)
                    {
                        ExecuteAction(selectedMi);
                    }
                    else
                    {
                        // use the positional value (only if the Command property does not have a value).
                        if (int.TryParse(value, out int option))
                        {
                            var selectedIdx = option - 1;
                            if (selectedIdx > -1 && selectedIdx < menuItems.Count)
                            {
                                var mi = menuItems[selectedIdx];
                                if (string.IsNullOrWhiteSpace(mi.Command))
                                {
                                    ExecuteAction(mi);
                                }
                            }
                        }
                    }

                    if (terminate)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Execute the command and decide if its time to terminare the 
        /// command loop
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="mi"></param>
        /// <returns>
        /// true = terminate the loop
        /// false = keep asking for another command
        /// </returns>
        private void ExecuteAction(MenuItem mi)
        {
            mi.Action?.Invoke();

            if (!terminate)
            {
                ConsoleHelpers.HitEnterToContinue();
            }
        }
    }
}