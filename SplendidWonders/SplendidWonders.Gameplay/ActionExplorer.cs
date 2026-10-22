using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay
{
    public class ActionExplorer
    {
        public static List<GameAction>? GetValidActions(GameState gameState)
        {
            var validActions = new List<GameAction>();

            var returnTokensActions = GetReturnTokensActions(gameState);
            var selectLeaderActions = GetSelectLeaderActions(gameState);

            if (returnTokensActions.Any())
            {
                validActions.AddRange(returnTokensActions);
                return validActions;
            }
            else if (selectLeaderActions.Any())
            {
                validActions.AddRange(selectLeaderActions);
                return validActions;
            }
            else
            {
                validActions.AddRange(GetDrawThreeTokensActions(gameState));
                validActions.AddRange(GetDrawDoubleTokensActions(gameState));
                validActions.AddRange(GetReserveCardFromBoardActions(gameState));
                validActions.AddRange(GetReserveCardFromDeckActions(gameState));
            }

            return validActions;
        }

        private static IEnumerable<GameAction> GetDrawThreeTokensActions(GameState gameState)
        {
            List<DrawThreeTokensAction> validDrawThreeActions = new List<DrawThreeTokensAction>();

            bool[] emptyColors =
            {
                gameState.TokenSupply.Yellow == 0,
                gameState.TokenSupply.Blue == 0,
                gameState.TokenSupply.Green == 0,
                gameState.TokenSupply.Brown == 0,
                gameState.TokenSupply.Red == 0,
            };

            if (emptyColors.Count(true) < 2)
            {
                var permutationActions = CalculateThreeTokensActionPermutations(emptyColors);
                validDrawThreeActions.AddRange(permutationActions);
            }
            else
            {
                var values = new int[5];

                for (int i = 0; i < emptyColors.Length; i++)
                {
                    values[i] = emptyColors[i] ? 0 : 1;
                }

                validDrawThreeActions.Add(new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray(values) });
            }

            return validDrawThreeActions;
        }

        private static IEnumerable<GameAction> GetDrawDoubleTokensActions(GameState gameState)
        {
            List<DrawDoubleTokensAction> gameActions = new List<DrawDoubleTokensAction>();

            if (gameState.TokenSupply.Yellow >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 2, 0, 0, 0, 0 }) });
            if (gameState.TokenSupply.Blue >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 2, 0, 0, 0 }) });
            if (gameState.TokenSupply.Green >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 2, 0, 0 }) });
            if (gameState.TokenSupply.Brown >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 2, 0 }) });
            if (gameState.TokenSupply.Red >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 0, 2 }) });

            return gameActions;
        }

        private static IEnumerable<GameAction> GetReturnTokensActions(GameState gameState)
        {
            List<ReturnTokensAction> validReturnTokensActions = new List<ReturnTokensAction>();

            var playerStore = gameState.CurrentPlayer.Store;
            var tokenCount = playerStore.Yellow + playerStore.Blue + playerStore.Green + playerStore.Brown + playerStore.Red + playerStore.Polymath;

            if (tokenCount > 10)
            {
                List<ReturnTokensAction> returnTokensActions = new List<ReturnTokensAction>();

                var current = new int[5];

                var availableTokens = new[]
                {
                    playerStore.Yellow,
                    playerStore.Blue,
                    playerStore.Green,
                    playerStore.Brown,
                    playerStore.Red
                };

                GenerateVariations(availableTokens, current, 0, tokenCount - 10, returnTokensActions);
                validReturnTokensActions.AddRange(returnTokensActions);
            }

            return validReturnTokensActions;
        }

        private static IEnumerable<GameAction> GetReserveCardFromBoardActions(GameState gameState)
        {
            List<ReserveFromBoardAction> reserveFromBoardActions = new List<ReserveFromBoardAction>();

            for (int i = 1; i < 4; i++)
            {
                var cardList = gameState.Market[i];

                for (int j = 0; j < cardList.Count; j++)
                {
                    var card = cardList[j];

                    reserveFromBoardActions.Add(new ReserveFromBoardAction() { Card = card });
                }
            }

            return reserveFromBoardActions;
        }

        private static IEnumerable<GameAction> GetReserveCardFromDeckActions(GameState gameState)
        {
            List<ReserveFromDeckAction> reserveFromDeckActions = new List<ReserveFromDeckAction>();

            for (int i = 1; i < 4; i++)
            {
                var deck = gameState.Decks[i];

                if (deck.Count > 0)
                {
                    reserveFromDeckActions.Add(new ReserveFromDeckAction() { Level = i });
                }
            }

            return reserveFromDeckActions;
        }

        private static IEnumerable<GameAction> GetSelectLeaderActions(GameState gameState)
        {
            List<SelectLeaderAction> selectLeaderActions = new List<SelectLeaderAction>();

            foreach (var leader in gameState.Leaders)
            {
                Dictionary<DevelopmentBonus, int> requirements = new Dictionary<DevelopmentBonus, int>()
                {
                    { DevelopmentBonus.Yellow, leader.Requirements.Yellow },
                    { DevelopmentBonus.Blue, leader.Requirements.Blue },
                    { DevelopmentBonus.Green, leader.Requirements.Green },
                    { DevelopmentBonus.Brown, leader.Requirements.Brown },
                    { DevelopmentBonus.Red, leader.Requirements.Red },
                };

                bool hasFailedRequirement = false;

                foreach (var requirement in requirements)
                {
                    if (requirement.Value == 0)
                        continue;

                    if (gameState.CurrentPlayer.Tableau.Count(card => card.Bonus == requirement.Key) < requirement.Value)
                    {
                        hasFailedRequirement = true;
                        break;
                    }
                }

                if (!hasFailedRequirement)
                {
                    selectLeaderActions.Add(new SelectLeaderAction() { Leader = leader });
                }
            }

            return selectLeaderActions;
        }

        private static IEnumerable<DrawThreeTokensAction> CalculateThreeTokensActionPermutations(bool[] emptyColors)
        {
            List<DrawThreeTokensAction> permutationActions = new List<DrawThreeTokensAction>();

            for (int i = 0; i < 5; i++)
            {
                if (emptyColors[i])
                    continue;

                for (int j = i + 1; j < 5; j++)
                {
                    if (emptyColors[j])
                        continue;

                    for (int k = j + 1; k < 5; k++)
                    {
                        if (emptyColors[k])
                            continue;

                        var values = new int[5];
                        values[i] = 1;
                        values[j] = 1;
                        values[k] = 1;

                        var group = TokenGroup.FromArray(values);
                        permutationActions.Add(new DrawThreeTokensAction() { Tokens = group });
                    }
                }
            }

            return permutationActions;
        }

        private static void GenerateVariations(
                int[] available,
                int[] current,
                int colorIndex,
                int remaining,
                List<ReturnTokensAction> result)
        {
            {
                // All tokens have been allocated.
                if (colorIndex == 5)
                {
                    if (remaining == 0)
                    {
                        result.Add(
                            new ReturnTokensAction()
                            {
                                Tokens = TokenGroup.FromArray([current[0], current[1], current[2], current[3], current[4]])
                            });
                    }

                    return;
                }

                // Try every possible quantity of this color.
                int max = Math.Min(available[colorIndex], remaining);

                for (int count = 0; count <= max; count++)
                {
                    current[colorIndex] = count;
                    GenerateVariations(available, current, colorIndex + 1, remaining - count, result);
                }

                current[colorIndex] = 0;
            }
        }

        public static GameState ReduceState(GameState gameState, GameAction command)
        {
            throw new NotImplementedException();
        }
    }
}