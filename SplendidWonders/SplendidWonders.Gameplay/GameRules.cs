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
            List<DrawThreeTokensAction> actions = new List<DrawThreeTokensAction>();

            if (gameState.TokenSupply.Yellow > 0 
                && gameState.TokenSupply.Blue > 0 
                && gameState.TokenSupply.Green > 0 
                && gameState.TokenSupply.Brown > 0 
                && gameState.TokenSupply.Red > 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = i + 1; j < 5; j++)
                    {
                        for (int k = j + 1; k < 5; k++)
                        {
                            var values = new int[5];
                            values[i] = 1;
                            values[j] = 1;
                            values[k] = 1;

                            var group = TokenGroup.FromArray(values);
                            actions.Add(new DrawThreeTokensAction() { Tokens = group });
                        }
                    }
                }
            }

            return actions;
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