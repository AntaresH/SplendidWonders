using SplendidWonders.CLI.Properties;
using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Commands;
using System;
using System.Collections.Generic;

namespace SplendidWonders.CLI.View
{
    public class ConsoleGameView : IGameView
    {
        private readonly ConsoleGameStateRenderer _gameStateRenderer = new();

        public void DisplayGameOver(Dictionary<string, int> value)
        {
            Console.WriteLine(Resources.GameOverTitle);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(Resources.FinalScores);

            foreach (var item in value.OrderBy(x => x.Value).Reverse())
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        public void DisplayGameState(GameState gameState)
        {
            _gameStateRenderer.Render(gameState);
        }

        public void DisplayValidActions(List<GameAction> validActions)
        {
            Console.WriteLine(Resources.ValidCommands);

            foreach (var command in validActions)
            {
                string shorthand = GenericUtils.CommandShortHandMap.ContainsKey(command.GetType().Name) ? GenericUtils.CommandShortHandMap[command.GetType().Name] : string.Empty;
                Console.WriteLine($"{command.GetType().Name} ({shorthand})");
            }
        }

        public void DisplayTurnStart(Player player)
        {
            Console.WriteLine(string.Format(Resources.PlayerTurnFormat, player.Name));
            Console.WriteLine();
        }

        public void DisplayInvalidCommandMessage()
        {
            Console.WriteLine(Resources.InvalidCommandState);
            Console.WriteLine();
        }

        public void DisplayCommandCategoryRequest(List<string> validActionCategories)
        {
            Console.WriteLine(string.Format(Resources.EnterCommandFromChoicesFormat, String.Join(", ", validActionCategories)));
            Console.WriteLine();
        }

        public void DisplayThreeTokensArgumentRequest()
        {
            Console.WriteLine(Resources.PromptChooseColors);
            Console.WriteLine();
        }

        public void DisplayDoubleTokensArgumentRequest()
        {
            Console.WriteLine(Resources.PromptChooseDoubleColor);
            Console.WriteLine();
        }

        public void DisplayPurchaseArgumentRequest()
        {
            Console.WriteLine(Resources.SelectCardToBuy);
            Console.WriteLine();
        }

        public void DisplayReserveFromDeckArgumentRequest()
        {
            Console.WriteLine(Resources.SelectDeckToReserve);
            Console.WriteLine();
        }

        public void DisplayReserveFromBoardArgumentRequest()
        {
            Console.WriteLine(Resources.SelectCardToReserve);
            Console.WriteLine();
        }

        public void DisplayInvalidCommand(GameAction command)
        {
            Console.WriteLine(string.Format(Resources.InvalidCommandTypeStateFormat, command.GetType().Name));
        }

        public void DisplayGameTitle()
        {
            Console.WriteLine("   _____       _                _ _     _  __          __             _               ");
            Console.WriteLine("  / ____|     | |              | (_)   | | \\ \\        / /            | |              ");
            Console.WriteLine(" | (___  _ __ | | ___ _ __   __| |_  __| |  \\ \\  /\\  / /__  _ __   __| | ___ _ __ ___ ");
            Console.WriteLine("  \\___ \\| '_ \\| |/ _ \\ '_ \\ / _` | |/ _` |   \\ \\/  \\/ / _ \\| '_ \\ / _` |/ _ \\ '__/ __|");
            Console.WriteLine("  ____) | |_) | |  __/ | | | (_| | | (_| |    \\  /\\  / (_) | | | | (_| |  __/ |  \\__ \\");
            Console.WriteLine(" |_____/| .__/|_|\\___|_| |_|\\__,_|_|\\__,_|     \\/  \\/ \\___/|_| |_|\\__,_|\\___|_|  |___/");
            Console.WriteLine("        | |                                                                           ");
            Console.WriteLine("        |_|                                                                           ");
            Console.WriteLine("");
        }
    }
}
