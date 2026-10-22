using SplendidWonders.CLI.View;
using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SplendidWonders.CLI.Input
{
    public class ConsoleGameInput : IGameInput
    {
        private IGameView _gameView;

        public ConsoleGameInput(IGameView gameView)
        {
            _gameView = gameView;
        }

        public GameAction ReadCommand(GameState gameState, List<GameAction> validActions)
        {
            while (true)
            {
                try
                {
                    var validActionCategories = validActions.Select(x => GenericUtils.CommandShortHandMap[x.GetType().Name]).Distinct().ToList();

                    string userInput = ReadAndApplyDefaultHandling();

                    if (!GenericUtils.CommandShortHandMap.ContainsValue(userInput))
                        continue;

                    return CreateCommandFromUserInput(userInput, gameState);

                }
                catch (UserCancelException)
                {
                    continue;
                }
            }
        }
        private static string ReadAndApplyDefaultHandling()
        {
            string userInput = Console.ReadLine().Trim();

            while (string.IsNullOrWhiteSpace(userInput))
            {
                userInput = Console.ReadLine().Trim();
            }

            if (userInput?.Equals("V", StringComparison.OrdinalIgnoreCase) == true)
                throw new UserCancelException();

            if (userInput?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                throw new UserAbortException();

            return userInput;
        }

        private GameAction CreateCommandFromUserInput(string userInput, GameState gameState)
        {
            var commandKey = GenericUtils.CommandShortHandMap.FirstOrDefault(x => x.Value == userInput).Key;

            switch (commandKey)
            {
                case nameof(DrawThreeTokensAction):
                    return ResolveThreeTokens(new DrawThreeTokensAction());
                case nameof(DrawDoubleTokensAction):
                    return ResolveDoubleTokens(new DrawDoubleTokensAction());
                case nameof(ReserveFromBoardAction):
                    return ResolveReserveFromBoard(new ReserveFromBoardAction(), gameState);
                case nameof(ReserveFromDeckAction):
                    return ResolveReserveFromDeck(new ReserveFromDeckAction());
                case nameof(PurchaseAction):
                    return ResolvePurchase(new PurchaseAction(), gameState);
            }

            throw new InvalidOperationException();
        }

        private GameAction ResolvePurchase(PurchaseAction command, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayPurchaseArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^([1-3]),([1-4])$");

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    int column = int.Parse(match.Groups[2].Value);
                    command.Card = gameState.Market[level][column];

                    return command;
                }
            }
        }

        private GameAction ResolveReserveFromDeck(ReserveFromDeckAction command)
        {
            while (true)
            {
                _gameView.DisplayReserveFromDeckArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^([1-3])$");

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    command.Level = level;

                    return command;
                }
            }
        }

        private GameAction ResolveReserveFromBoard(ReserveFromBoardAction command, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayPurchaseArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^([1-3]),([1-4])$");

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    int column = int.Parse(match.Groups[2].Value);
                    command.Card = gameState.Market[level][column];

                    return command;
                }
            }
        }

        private GameAction ResolveDoubleTokens(DrawDoubleTokensAction command)
        {
            while (true)
            {
                _gameView.DisplayDoubleTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^[SKZBP]{1}$");

                if (match.Success)
                {
                    command.Tokens = new TokenGroup
                    {
                        Yellow = userInput.Contains('S') ? 2 : 0,
                        Blue = userInput.Contains('K') ? 2 : 0,
                        Green = userInput.Contains('Z') ? 2 : 0,
                        Brown = userInput.Contains('B') ? 2 : 0,
                        Red = userInput.Contains('P') ? 2 : 0
                    };

                    return command;
                }
            }
        }

        private GameAction ResolveThreeTokens(DrawThreeTokensAction action)
        {
            while (true)
            {
                _gameView.DisplayThreeTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^[SKZBP ]{1,3}$");

                if (match.Success)
                {
                    action.Tokens = new TokenGroup
                    {
                        Yellow = userInput.Contains('S') ? 1 : 0,
                        Blue = userInput.Contains('K') ? 1 : 0,
                        Green = userInput.Contains('Z') ? 1 : 0,
                        Brown = userInput.Contains('B') ? 1 : 0,
                        Red = userInput.Contains('P') ? 1 : 0
                    };

                    return action;
                }
            }
        }
    }
}
