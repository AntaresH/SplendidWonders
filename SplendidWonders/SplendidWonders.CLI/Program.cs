using SplendidWonders.CLI;
using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using System.Diagnostics;
using System.Runtime.InteropServices;
internal class Program
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_MAXIMIZE = 3;

    static void Main(string[] args)
    {
        // Maximize console window
        IntPtr handle = GetConsoleWindow();

        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_MAXIMIZE);
        }

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        
        IGameView gameView = new ConsoleGameView();

        gameView.DisplayGameTitle();

        bool stayInGame = true;

        while (stayInGame)
        {
            List<string> playerNames = new List<string>();

            for (int i = 1; i < 3; i++)
            {
                Console.WriteLine(string.Format(SplendidWonders.CLI.Properties.Resources.EnterPlayerNameFormat, i));
                var userInput = Console.ReadLine().Trim();

                while (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine(SplendidWonders.CLI.Properties.Resources.NameCannotBeEmpty);

                    userInput = Console.ReadLine();
                }

                playerNames.Add(userInput);
            }

            bool useExtendedLeaders = false;

            while (true)
            {
                Console.WriteLine(SplendidWonders.CLI.Properties.Resources.UseExtendedLeaders);

                string input = Console.ReadLine().Trim().ToLower();

                if (input == "i" || input == "igen")
                {
                    useExtendedLeaders = true;
                    break;
                }
                else if (input == "n" || input == "nem")
                {
                    useExtendedLeaders = false;
                    break;
                }
                else
                {
                    Console.WriteLine(SplendidWonders.CLI.Properties.Resources.InvalidYesNo);
                }
            }

            IGameInput gameInput = new ConsoleGameInput(gameView);
            GameOrchestrator executor = new GameOrchestrator(useExtendedLeaders, playerNames, gameView, gameInput);
            executor.Start();

            while (true)
            {
                Console.WriteLine(SplendidWonders.CLI.Properties.Resources.PlayAgain);

                var userInput = Console.ReadLine().Trim();

                if (userInput == "i" || userInput == "igen")
                {
                    stayInGame = true;
                    break;
                }
                else if (userInput == "n" || userInput == "nem")
                {
                    stayInGame = false;
                    break;
                }
            }
        }
    }
}

