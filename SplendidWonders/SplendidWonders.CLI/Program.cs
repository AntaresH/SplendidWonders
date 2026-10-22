using SplendidWonders.CLI;
using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using SplendidWonders.Common;
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
            //List<string> playerNames = PlayerPrompter();
            //bool useExtendedLeaders = LeaderPrompter();
            
            IGameInput gameInput = new ConsoleGameInput(gameView);
            GameOrchestrator orchestrator = new GameOrchestrator(true, new List<string>() { "Ambrus", "Brünhilda" }, gameView, gameInput);
            orchestrator.Start();

            stayInGame = StayPrompter();
        }
    }

    private static bool StayPrompter()
    {
        while (true)
        {
            Console.WriteLine(Strings.PlayAgain);

            var userInput = Console.ReadLine().Trim();

            if (userInput == "i" || userInput == "igen")
            {
                return true;
            }
            else if (userInput == "n" || userInput == "nem")
            {
                return false;
            }
        }
    }

    private static bool LeaderPrompter()
    {
        while (true)
        {
            Console.WriteLine(Strings.UseExtendedLeaders);

            string input = Console.ReadLine().Trim().ToLower();

            if (input == "i" || input == "igen")
            {
                return true;
            }
            else if (input == "n" || input == "nem")
            {
                return false;
            }
            else
            {
                Console.WriteLine(Strings.InvalidYesNo);
            }
        }
    }

    private static List<string> PlayerPrompter()
    {
        var playerNames = new List<string>();

        for (int i = 1; i < 3; i++)
        {
            Console.WriteLine(string.Format(Strings.EnterPlayerNameFormat, i));
            var userInput = Console.ReadLine().Trim();

            while (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine(Strings.NameCannotBeEmpty);

                userInput = Console.ReadLine();
            }

            playerNames.Add(userInput);
        }

        return playerNames;
    }
}

