using SplendidWonders.CLI;
using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;

Console.WriteLine("Splendid Wonders!");
Console.WriteLine();

bool stayInGame = true;

while (stayInGame) 
{ 
    int players = 0;

    while (players < 2 || players > 4)
    {
        Console.WriteLine("Kérlek add meg a játékosok számát (2-4):");
        Console.WriteLine();

        Int32.TryParse(Console.ReadLine(), out players);
    }

    List<string> playerNames = new List<string>();
    for (int i = 0; i < players; i++)
    {
        Console.WriteLine($"Kérlek add meg a {i + 1}. játékos nevét:");

        var userInput = Console.ReadLine().Trim();

        while (string.IsNullOrWhiteSpace(userInput))
        {
            Console.WriteLine("A név nem lehet üres. Kérlek add meg újra a játékos nevét:");
            userInput = Console.ReadLine();
        }

        playerNames.Add(Console.ReadLine().Trim());
    }

    bool useExtendedLeaders = false;

    while (true)
    {
        Console.WriteLine("Használod az extra vezetőket? (i/n):");
        Console.WriteLine();

        if (Boolean.TryParse(Console.ReadLine(), out useExtendedLeaders))
            break;
    }

    IGameView gameView = new ConsoleGameView();
    IGameInput gameInput = new ConsoleGameInput(gameView);
    GameExecutor executor = new GameExecutor(useExtendedLeaders, playerNames, gameView, gameInput);
    executor.Start();

    while (true)
    {
        Console.WriteLine("Szeretnél újra játszani? (i/n):");
        Console.WriteLine();

        if (Boolean.TryParse(Console.ReadLine(), out stayInGame))
            break;
    }
}

