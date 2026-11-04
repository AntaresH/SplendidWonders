using SplendidWonders.CLI.View;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace SplendidWonders.CLI.Input
{
    public class ConsoleGameInput : IGameInput
    {
        private IGameView _gameView;

        public ConsoleGameInput(IGameView gameView)
        {
            _gameView = gameView;
        }

        public GameAction ReadCommand(GameState gameState, IEnumerable<GameAction> validActions)
        {
            while (true)
            {
                try
                {
                    string userInput = ReadAndApplyDefaultHandling();

                    if (!GenericUtils.CommandShortHandMap.ContainsValue(userInput.ToUpper()))
                        continue;

                    if (validActions.Any(a => GenericUtils.CommandShortHandMap[a.GetType().Name] == userInput.ToUpper()))
                    {
                        return CreateCommandFromUserInput(userInput, gameState);
                    }
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
            var commandKey = GenericUtils.CommandShortHandMap.FirstOrDefault(x => x.Value == userInput.ToUpper()).Key;

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
                case nameof(PurchaseFromBoardAction):
                    return ResolvePurchaseFromBoard(new PurchaseFromBoardAction(), gameState);
                case nameof(PurchaseFromReservedAction):
                    return ResolvePurchaseFromReserved(new PurchaseFromReservedAction(), gameState);
                case nameof(ReturnTokensAction):
                    return ResolveReturnTokens(new ReturnTokensAction(), gameState);
                case nameof(SelectLeaderAction):
                    return ResolveSelectLeader(new SelectLeaderAction(), gameState);
            }

            throw new InvalidOperationException();
        }

        private GameAction ResolvePurchaseFromBoard(PurchaseAction action, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayPurchaseFromBoardArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^([1-3]),([1-4])$");

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    int column = int.Parse(match.Groups[2].Value);
                    action.Card = gameState[level][column];

                    if (action.Card == Cards.Empty)
                        continue;

                    return action;
                }
            }
        }

        private GameAction ResolvePurchaseFromReserved(PurchaseFromReservedAction action, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayPurchaseFromReservedArgumentRequest(gameState.ActivePlayer.ReservedCards);

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^#?([\d]){1,2}$");

                if (match.Success)
                {
                    int cardId = int.Parse(match.Groups[0].Value);

                    if (!gameState.ActivePlayer.ReservedCards.Any(c => c.Id == cardId))
                        continue;

                    action.Card = gameState.ActivePlayer.ReservedCards.First(c => c.Id == cardId);

                    return action;
                }
            }
        }

        private GameAction ResolveReserveFromDeck(ReserveFromDeckAction action)
        {
            while (true)
            {
                _gameView.DisplayReserveFromDeckArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^([1-3])$");

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    action.Level = level;

                    return action;
                }
            }
        }

        private GameAction ResolveReserveFromBoard(ReserveFromBoardAction action, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayPurchaseFromBoardArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^([1-3]),([1-4])$");

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    int column = int.Parse(match.Groups[2].Value);
                    action.Card = gameState[level][column];

                    if (action.Card == Cards.Empty)
                        continue;

                    return action;
                }
            }
        }

        private GameAction ResolveDoubleTokens(DrawDoubleTokensAction action)
        {
            while (true)
            {
                _gameView.DisplayDoubleTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^[SsKkZzBbPp]{1}$");

                if (match.Success)
                {
                    action.Tokens = new TokenGroup
                    {
                        Yellow = userInput.Contains('S', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Blue = userInput.Contains('K', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Green = userInput.Contains('Z', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Brown = userInput.Contains('B', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Red = userInput.Contains('P', StringComparison.OrdinalIgnoreCase) ? 2 : 0
                    };

                    return action;
                }
            }
        }

        private GameAction ResolveThreeTokens(DrawThreeTokensAction action)
        {
            while (true)
            {
                _gameView.DisplayThreeTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^[SKZBPskzbp]+( [SKZBPskzbp])*( [SKZBPskzbp])*$");

                if (match.Success)
                {
                    action.Tokens = new TokenGroup
                    {
                        Yellow = userInput.Contains('S', StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                        Blue = userInput.Contains('K', StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                        Green = userInput.Contains('Z', StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                        Brown = userInput.Contains('B', StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                        Red = userInput.Contains('P', StringComparison.OrdinalIgnoreCase) ? 1 : 0
                    };

                    return action;
                }
            }
        }

        private GameAction ResolveReturnTokens(ReturnTokensAction action, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayReturnTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();

                Match match = Regex.Match(userInput, @"^[SKZBPskzbp]+( [SKZBPskzbp])*( [SKZBPskzbp])*$");

                if (match.Success)
                {
                    action.Tokens = new TokenGroup
                    {
                        Yellow = userInput.Count(c => c == 'S' || c == 's'),
                        Blue = userInput.Count(c => c == 'K' || c == 'k'),
                        Green = userInput.Count(c => c == 'Z' || c == 'z'),
                        Brown = userInput.Count(c => c == 'B' || c == 'b'),
                        Red = userInput.Count(c => c == 'P' || c == 'p')
                    };

                    return action;
                }
            }
        }

        private GameAction ResolveSelectLeader(SelectLeaderAction selectLeaderAction, GameState gameState)
        {
            throw new NotImplementedException();
        }
    }
}
