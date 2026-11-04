using SplendidWonders.Common;
using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SplendidWonders.CLI.View
{
    public class ConsoleGameView : IGameView
    {
        private readonly ConsoleGameStateRenderer _gameStateRenderer = new();
        private readonly ConsoleGameActionsRenderer _gameActionsRenderer = new();

        public void DisplayGameOver(Dictionary<string, int> value)
        {
            Console.WriteLine(Strings.GameOverTitle);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(Strings.FinalScores);

            foreach (var item in value.OrderBy(x => x.Value).Reverse())
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        public void DisplayGame(GameState gameState, IEnumerable<GameAction> validActions, string previousInvalidCommand)
        {
            Console.Clear();
            DisplayGameTitle();
            _gameStateRenderer.Render(gameState);

            if (!string.IsNullOrEmpty(previousInvalidCommand))
            {
                Console.WriteLine();
                Console.WriteLine(previousInvalidCommand);
                Console.WriteLine();
            }

            _gameActionsRenderer.Render(validActions);
        }

        public void DisplayValidActions(IEnumerable<GameAction> validActions)
        {
            Console.WriteLine(Strings.ValidCommands);

            foreach (var command in validActions)
            {
                string shorthand = GenericUtils.CommandShortHandMap.ContainsKey(command.GetType().Name) ? GenericUtils.CommandShortHandMap[command.GetType().Name] : string.Empty;
                Console.WriteLine($"{command.GetType().Name} ({shorthand})");
            }
        }

        public void DisplayTurnStart(Player player)
        {
            Console.WriteLine(string.Format(Strings.PlayerTurnFormat, player.Name));
        }

        public void DisplayInvalidCommandMessage()
        {
            Console.WriteLine(Strings.InvalidCommandState);
        }

        public void DisplayThreeTokensArgumentRequest()
        {
            Console.WriteLine(Strings.PromptChooseColors);
        }

        public void DisplayDoubleTokensArgumentRequest()
        {
            Console.WriteLine(Strings.PromptChooseDoubleColor);
        }

        public void DisplayPurchaseFromBoardArgumentRequest()
        {
            Console.WriteLine(Strings.SelectCardToBuy);
        }

        public void DisplayReserveFromDeckArgumentRequest()
        {
            Console.WriteLine(Strings.SelectDeckToReserve);
        }

        public void DisplayReserveFromBoardArgumentRequest()
        {
            Console.WriteLine(Strings.SelectCardToReserve);
        }

        public void DisplayInvalidCommand(GameAction command)
        {
            Console.WriteLine(string.Format(Strings.InvalidCommandTypeStateFormat, command.GetType().Name));
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

        public void DisplayReturnTokensArgumentRequest()
        {
            Console.WriteLine(Strings.PromptReturnTokens);
        }

        public void DisplayPurchaseFromReservedArgumentRequest(ImmutableList<DevelopmentCard> reservedCards)
        {
            Console.WriteLine(Strings.SelectCardToBuyFromReserved);
            foreach (var card in reservedCards)
            {
                Console.Write($"{card.Id} \u001b[38;2;{ConvertRgbColorIntoText(card.Bonus)}m{card.Name}\u001b[0m Költségek: ");

                if (card.Cost.Yellow > 0)
                {
                    Console.Write($"\u001b[38;2;{ConvertRgbColorIntoText(DevelopmentBonus.Yellow)}m{card.Cost.Yellow}\u001b[0m ");
                }

                if (card.Cost.Blue > 0)
                {
                    Console.Write($"\u001b[38;2;{ConvertRgbColorIntoText(DevelopmentBonus.Blue)}m{card.Cost.Blue}\u001b[0m ");
                }

                if (card.Cost.Green > 0)
                {
                    Console.Write($"\u001b[38;2;{ConvertRgbColorIntoText(DevelopmentBonus.Green)}m{card.Cost.Green}\u001b[0m ");
                }

                if (card.Cost.Brown > 0)
                {
                    Console.Write($"\u001b[38;2;{ConvertRgbColorIntoText(DevelopmentBonus.Brown)}m{card.Cost.Brown}\u001b[0m ");
                }

                if (card.Cost.Red > 0)
                {
                    Console.Write($"\u001b[38;2;{ConvertRgbColorIntoText(DevelopmentBonus.Red)}m{card.Cost.Red}\u001b[0m ");
                }
            }
        }

        private static string ConvertRgbColorIntoText(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => "208;208;0",
                DevelopmentBonus.Blue => "80;145;255",
                DevelopmentBonus.Green => "0;220;50",
                DevelopmentBonus.Brown => "145;92;42",
                DevelopmentBonus.Red => "255;80;80",
                _ => "192;192;192"
            };
        }
    }
}
