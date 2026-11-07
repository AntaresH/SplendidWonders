using SplendidWonders.CLI.Input;
using SplendidWonders.Common;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.WebSockets;

namespace SplendidWonders.CLI.View
{
    public class ConsoleGameView : IGameView
    {
        private readonly ConsoleGameStateRenderer _gameStateRenderer = new();
        private readonly ConsoleGameActionsRenderer _gameActionsRenderer = new();

        public void DisplayGameOver(List<Player> players)
        {
            Console.WriteLine(Strings.GameOverTitle);
            Console.WriteLine();
            Console.WriteLine($"{Strings.Winner}: {players.First().Name}");
            Console.WriteLine();
            Console.WriteLine(Strings.FinalScores);
            Console.WriteLine();

            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine($"#{i+1}: {players[i].Name} - {players[i].Tableau.Sum(c => c.VictoryPoints) + players[i].AcquiredLeaders.Count * 3} {Strings.VP}");
            }
        }

        public void DisplayGame(GameState gameState, IEnumerable<GameAction> validActions, string previousInvalidCommand)
        {
            DisplayBoard(gameState);

            if (!string.IsNullOrEmpty(previousInvalidCommand))
            {
                Console.WriteLine();
                PrintColoredLine(previousInvalidCommand, ConsoleColor.Red);
                Console.WriteLine();
                Console.WriteLine();
            }
            _gameActionsRenderer.Render(gameState, validActions);
        }

        public void DisplayBoard(GameState gameState)
        {
            Console.Clear();
            DisplayGameTitle();
            _gameStateRenderer.Render(gameState);
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

        public void DisplayPurchaseFromBoardArgumentRequest(IEnumerable<DevelopmentCard> purchasableCards)
        {
            Console.WriteLine(Strings.SelectCardToBuy);

            DisplayCards(purchasableCards);
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

            DisplayCards(reservedCards);
        }

        public static string GetRgbColoredConsoleForeground(string text, DevelopmentBonus trueColorRgbFormat)
        {
            return $"\u001b[38;2;{ConvertRgbColorIntoText(trueColorRgbFormat)}m{text}\u001b[0m";
        }

        public static string GetRgbColoredConsoleBackground(string text, DevelopmentBonus trueColorRgbFormat)
        {
            return $"\u001b[48;2;{ConvertRgbColorIntoText(trueColorRgbFormat)}m{text}\u001b[0m";
        }

        public static string ConvertRgbColorIntoText(DevelopmentBonus bonus)
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

        public void DisplayPresentationCommand(ApplicationCommand command, GameState gameState)
        {
            if (command is DisplayReservedCardsCommand)
            {
                DisplayCards(gameState.ActivePlayer.ReservedCards);
                Console.ReadLine();
            }
        }

        public void DisplaySelectLeaderArgumentRequest(IEnumerable<Leader> eligibleLeaders)
        {
            foreach (var leader in eligibleLeaders)
            {
                PrintLeader(leader);
            }
        }

        private void DisplayCards(IEnumerable<DevelopmentCard> cards)
        {
            foreach (var card in cards)
            {
                PrintCard(card);
            }
        }

        public static void PrintCard(DevelopmentCard card)
        {
            List<string> colorCostTexts = new List<string>();

            if (card.Cost.Yellow > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Yellow.ToString(), DevelopmentBonus.Yellow));
            }

            if (card.Cost.Blue > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Blue.ToString(), DevelopmentBonus.Blue));
            }

            if (card.Cost.Green > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Green.ToString(), DevelopmentBonus.Green));
            }

            if (card.Cost.Brown > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Brown.ToString(), DevelopmentBonus.Brown));
            }

            if (card.Cost.Red > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Red.ToString(), DevelopmentBonus.Red));
            }

            string vpDisplay = card.VictoryPoints > 0 ? $"{{{card.VictoryPoints}}}" : "   ";

            string namePadding = new string(' ', Math.Max(0, 20 - card.Name.Length));
            string coloredName = GetRgbColoredConsoleForeground(card.Name, card.Bonus);

            Console.WriteLine($"{card.Id,-5} {coloredName}{namePadding} {vpDisplay} Költségek: {string.Join(" ", colorCostTexts)}");
        }

        public static void PrintLeader(Leader leader)
        {
            List<string> colorCostTexts = new List<string>();

            if (leader.Requirements.Yellow > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Yellow.ToString()} ", DevelopmentBonus.Yellow));
            }

            if (leader.Requirements.Blue > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Blue.ToString()} ", DevelopmentBonus.Blue));
            }

            if (leader.Requirements.Green > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Green.ToString()} ", DevelopmentBonus.Green));
            }

            if (leader.Requirements.Brown > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Brown.ToString()} ", DevelopmentBonus.Brown));
            }

            if (leader.Requirements.Red > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Red.ToString()} ", DevelopmentBonus.Red));
            }

            string vpDisplay = leader.VictoryPoints > 0 ? $"{{{leader.VictoryPoints}}}" : string.Empty;

            Console.WriteLine($"{leader.Id} {leader.Name.PadRight(15)} {vpDisplay} Feltételek: {string.Join(" ", colorCostTexts)}");
        }

        public static void PrintColoredLine(string text, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }
    }
}
