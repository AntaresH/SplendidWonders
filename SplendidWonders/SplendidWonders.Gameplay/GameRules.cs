using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay
{
    public class GameRules
    {
        public static List<GameAction>? GetValidActions(GameState gameState)
        {
            var validActions = new List<GameAction>();

            validActions.AddRange(GetDrawThreeTokensActions(gameState));
            validActions.AddRange(GetDrawDoubleTokensActions(gameState));

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

        private static IEnumerable<GameAction> GetDrawDoubleTokensActions(GameState gameState)
        {
            List<DrawDoubleTokensAction> gameActions = new List<DrawDoubleTokensAction>();

            if (gameState.TokenSupply.Yellow >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 1, 0, 0, 0, 0 }) });
            if (gameState.TokenSupply.Blue >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 1, 0, 0, 0 }) });
            if (gameState.TokenSupply.Green >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 1, 0, 0 }) });
            if (gameState.TokenSupply.Brown >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 1, 0 }) });
            if (gameState.TokenSupply.Red >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 0, 1 }) });

            return gameActions;
        }

        public static GameState ReduceState(GameState gameState, GameAction command)
        {
            throw new NotImplementedException();
        }
    }
}