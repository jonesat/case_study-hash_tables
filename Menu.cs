using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Array;
using static System.Math;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.Design;
using System.Threading;
using System.Numerics;

namespace HashTableExploration
{
    public class Menu
    {
        protected string title;
        protected string subtitle;
        protected List<string> menuOptions;
        protected List<ICommand> commandList;

        public Menu(string title, string subtitle)
        {
            this.title = title;
            this.subtitle = subtitle;
            this.menuOptions = new List<string>();
            this.commandList = new List<ICommand>();
        }

        public virtual void DisplayMenu()
        {
            Clear();
            DisplayHeader();
            DisplayOptions();
            string input = GetInput();

            if (input == "0")
            {
                EndProgramCommand endProgramCommand = new EndProgramCommand();
                endProgramCommand.Execute();
            }
            else
            {
                ExecuteCommand(input);
            }

        }

        protected virtual void DisplayHeader()
        {
            int windowWidth = WindowWidth;
            string line = new string('#', windowWidth);
            WriteLine(line);
            int titleLength = title.Length;
            int leftMargin = (windowWidth - titleLength) / 2;
            SetCursorPosition(leftMargin, Console.CursorTop);
            WriteLine(title);

            
            int subtitleLength = subtitle.Length;
            int subtitleLeftMargin = (Console.WindowWidth - subtitleLength) / 2;
            WriteLine();
            SetCursorPosition(subtitleLeftMargin, Console.CursorTop);
            WriteLine(subtitle);
            WriteLine(line);
        }

        protected virtual void DisplayOptions()
        {
            WriteLine("Select an option or press 0 to exit:");
            for (int i = 0; i < menuOptions.Count; i++)
            {
                WriteLine($"{i + 1}. {menuOptions[i]}");
            }
        }

        protected virtual string GetInput()
        {
            Write(">> ");
            return ReadLine();
        }

        protected virtual void ExecuteCommand(string input)
        {
            int option;
            if (int.TryParse(input, out option) && option >= 1 && option <= menuOptions.Count)
            {
                ICommand command = commandList[option - 1];
                command.Execute();
                WriteLine($"Return to {subtitle} menu - press any key");
                ReadKey();
                Clear();
                
            }
            else
            {
                WriteLine("Invalid option. Please try again.");
                DisplayMenu();
            }
        }

        public void AddOption(string option, ICommand command)
        {
            if (!menuOptions.Contains(option))
            {
                menuOptions.Add(option);
                commandList.Add(command);
            }
        }

        public void RemoveOption(string option)
        {
            int optionIndex = menuOptions.IndexOf(option);
            if (optionIndex != -1)
            {
                menuOptions.RemoveAt(optionIndex);
                commandList.RemoveAt(optionIndex);
            }
        }

        public int OptionsCount()
        {
            return menuOptions.Count;
        }
    }
    public class HubMenu: Menu
    {
        public HubMenu(string title, string subtitle) : base(title, subtitle)
        {
        }

        protected override void ExecuteCommand(string input)
        {
            int option;
            if (int.TryParse(input, out option) && option >= 1 && option <= menuOptions.Count)
            {
                ICommand command = commandList[option - 1];
                command.Execute();
                WriteLine($"Return to {subtitle} menu - press any key");
                ReadKey();
                Clear();                
            }
            else
            {
                WriteLine("Invalid option. Please try again.");
                DisplayMenu();
            }
        }
    }
}
