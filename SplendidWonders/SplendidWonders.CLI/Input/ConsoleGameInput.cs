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

        private static string _lastArguments;

        public ConsoleGameInput(IGameView gameView)
        {
            _gameView = gameView;
        }

        public ApplicationCommand ReadCommand(GameState gameState, IEnumerable<GameAction> validActions)
        {
            while (true)
            {
                try
                {
                    string userInput = ReadAndApplyDefaultHandling();

                    if (!GenericUtils.CommandShortHandMap.ContainsValue(userInput.ToUpper()))
                        continue;

                    var commandKey = GenericUtils.CommandShortHandMap.FirstOrDefault(x => x.Value == userInput.ToUpper()).Key;

                    if (commandKey == nameof(DisplayReservedCardsCommand))
                    {
                        return new DisplayReservedCardsCommand
                        {
                            CommandText = userInput,
                        };
                    }

                    if (validActions.Any(a => GenericUtils.CommandShortHandMap[a.GetType().Name] == userInput.ToUpper()))
                    {
                        return CreateGameCommandFromUserInput(userInput, gameState, validActions);
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

        private ApplicationCommand CreateGameCommandFromUserInput(string userInput, GameState gameState, IEnumerable<GameAction> validActions)
        {
            var commandKey = GenericUtils.CommandShortHandMap.FirstOrDefault(x => x.Value == userInput.ToUpper()).Key;

            return new GameCommand
            {
                Action = CreateAction(commandKey, gameState, validActions),
                CommandText = userInput + " " + _lastArguments,
            };
        }

        private GameAction CreateAction(string commandKey, GameState gameState, IEnumerable<GameAction> validActions)
        {
            return commandKey switch
            {
                nameof(DrawThreeTokensAction) => CreateThreeTokens(new DrawThreeTokensAction()),
                nameof(DrawDoubleTokensAction) => CreateDoubleTokens(new DrawDoubleTokensAction()),
                nameof(ReserveFromBoardAction) => CreateReserveFromBoard(new ReserveFromBoardAction(), gameState),
                nameof(ReserveFromDeckAction) => CreateReserveFromDeck(new ReserveFromDeckAction()),
                nameof(PurchaseFromBoardAction) => CreatePurchaseFromBoard(new PurchaseFromBoardAction(), gameState, validActions),
                nameof(PurchaseFromReservedAction) => CreatePurchaseFromReserved(new PurchaseFromReservedAction(), gameState),
                nameof(ReturnTokensAction) => CreateReturnTokens(new ReturnTokensAction(), gameState),
                nameof(SelectLeaderAction) => CreateSelectLeader(new SelectLeaderAction(), gameState, validActions),
                _ => throw new InvalidOperationException(),
            };
        }

        private GameAction CreatePurchaseFromBoard(PurchaseAction action, GameState gameState, IEnumerable<GameAction> validActions)
        {
            while (true)
            {
                IEnumerable<DevelopmentCard> purchasableCards = validActions.OfType<PurchaseFromBoardAction>().Select(a => a.Card);
                _gameView.DisplayPurchaseFromBoardArgumentRequest(purchasableCards);

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

                Match match = Regex.Match(userInput, @"^#?([\d]){1,2}$");

                if (match.Success)
                {
                    int cardId = int.Parse(match.Groups[0].Value);

                    if (!purchasableCards.Any(c => c.Id == cardId))
                        continue;

                    action.Card = purchasableCards.First(c => c.Id == cardId);

                    return action;
                }
            }
        }

        private GameAction CreatePurchaseFromReserved(PurchaseFromReservedAction action, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayPurchaseFromReservedArgumentRequest(gameState.ActivePlayer.ReservedCards);

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

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

        private GameAction CreateReserveFromDeck(ReserveFromDeckAction action)
        {
            while (true)
            {
                _gameView.DisplayReserveFromDeckArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

                Match match = Regex.Match(userInput, @"^([1-3])$");

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    action.Level = level;

                    return action;
                }
            }
        }

        private GameAction CreateReserveFromBoard(ReserveFromBoardAction action, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayReserveFromBoardArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

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

        private GameAction CreateDoubleTokens(DrawDoubleTokensAction action)
        {
            while (true)
            {
                _gameView.DisplayDoubleTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

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

        private GameAction CreateThreeTokens(DrawThreeTokensAction action)
        {
            while (true)
            {
                _gameView.DisplayThreeTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

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

        private GameAction CreateReturnTokens(ReturnTokensAction action, GameState gameState)
        {
            while (true)
            {
                _gameView.DisplayReturnTokensArgumentRequest();

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

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

        private GameAction CreateSelectLeader(SelectLeaderAction action, GameState gameState, IEnumerable<GameAction> validActions)
        {
            while (true)
            {
                IEnumerable<Leader> eligibleLeaders = validActions.OfType<SelectLeaderAction>().Select(a => a.Leader);
                _gameView.DisplaySelectLeaderArgumentRequest(eligibleLeaders);

                string userInput = ReadAndApplyDefaultHandling();
                _lastArguments = userInput;

                Match match = Regex.Match(userInput, @"^#?([\d]){1,2}$");

                if (match.Success)
                {
                    int cardId = int.Parse(match.Groups[0].Value);

                    if (!eligibleLeaders.Any(c => c.Id == cardId))
                        continue;

                    action.Leader = eligibleLeaders.First(c => c.Id == cardId);

                    return action;
                }
            }
        }
    }
}
