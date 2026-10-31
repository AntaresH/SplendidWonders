using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI;
using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using SplendidWonders.Common;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
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

        var services = new ServiceCollection();

        services.AddSingleton<IActionExplorer<DrawThreeTokensAction>, DrawThreeTokensExplorer>();
        services.AddSingleton<IActionExplorer<DrawDoubleTokensAction>, DrawDoubleTokensExplorer>();
        services.AddSingleton<IActionExplorer<ReturnTokensAction>, ReturnTokensExplorer>();
        services.AddSingleton<IActionExplorer<ReserveFromBoardAction>, ReserveFromBoardExplorer>();
        services.AddSingleton<IActionExplorer<ReserveFromDeckAction>, ReserveFromDeckExplorer>();
        services.AddSingleton<IActionExplorer<PurchaseAction>, PurchaseExplorer>();
        services.AddSingleton<IActionExplorer<SelectLeaderAction>, SelectLeaderExplorer>();
        services.AddSingleton<IActionExplorer, ActionExplorer>();

        services.AddSingleton<TokenActionReducer>();
        services.AddSingleton<IStateReducer<DrawThreeTokensAction>>(sp => sp.GetRequiredService<TokenActionReducer>());
        services.AddSingleton<IStateReducer<DrawDoubleTokensAction>>(sp => sp.GetRequiredService<TokenActionReducer>());
        services.AddSingleton<IStateReducer<ReturnTokensAction>>(sp => sp.GetRequiredService<TokenActionReducer>());

        services.AddSingleton<ReserveActionReducer>();
        services.AddSingleton<IStateReducer<ReserveFromBoardAction>>(sp => sp.GetRequiredService<ReserveActionReducer>());
        services.AddSingleton<IStateReducer<ReserveFromBoardAction>>(sp => sp.GetRequiredService<ReserveActionReducer>());

        services.AddSingleton<IStateReducer<PurchaseAction>, PurchaseReducer>();
        services.AddSingleton<IStateReducer<SelectLeaderAction>, SelectLeaderReducer>();
        services.AddSingleton<IStateReducer, StateReducer>();

        services.AddSingleton<IGameView, ConsoleGameView>();
        services.AddSingleton<IGameInput, ConsoleGameInput>();

        var serviceProvider = services.BuildServiceProvider();

        bool stayInGame = true;

        while (stayInGame)
        {
            //List<string> playerNames = PlayerPrompter();
            //bool useExtendedLeaders = LeaderPrompter();
            
            GameConfiguration gameConfiguration = new GameConfiguration()
            {
                PlayerNames = new List<string>() { "Ambrus", "Brünhilda" },
                UseExtendedLeaders = true,
                UseRandomSeed = true
            };

            GameOrchestrator orchestrator = ActivatorUtilities.CreateInstance<GameOrchestrator>(serviceProvider, gameConfiguration);
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

