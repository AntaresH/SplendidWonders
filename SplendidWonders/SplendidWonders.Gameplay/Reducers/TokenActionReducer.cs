using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class TokenActionReducer : IStateReducer<DrawDoubleTokensAction>, IStateReducer<DrawThreeTokensAction>, IStateReducer<ReturnTokensAction>
    {
        private GameState ReduceGeneralTokenAction(GameState baseState, int[] selectedTokens)
        {
            var playerStore = baseState.CurrentPlayer.Store.ToArray();
            var tokenSupply = baseState.TokenSupply.ToArray();

            var reducedStateBuilder = new GameStateBuilder(baseState);

            for (int i = 0; i < 5; i++)
            {
                playerStore[i] += selectedTokens[i];
                tokenSupply[i] -= selectedTokens[i];
            }

            reducedStateBuilder.WithPlayerStore(baseState.CurrentPlayer.Id, TokenGroup.FromArray(playerStore));
            reducedStateBuilder.WithTokenSupply(TokenGroup.FromArray(tokenSupply));

            int allPlayerTokens = playerStore[0] + playerStore[1] + playerStore[2] + playerStore[3] + playerStore[4] + playerStore[5];
            var currentPlayerId = allPlayerTokens <= 10
                ? baseState.CurrentPlayer.Id + 1 % baseState.Players.Count
                : baseState.CurrentPlayer.Id;

            reducedStateBuilder.WithCurrentPlayer(currentPlayerId);
            return reducedStateBuilder.Build();
        }

        public GameState Reduce(GameState state, DrawThreeTokensAction action)
        {
            return ReduceGeneralTokenAction(state, action.Tokens.ToArray());
        }

        public GameState Reduce(GameState state, DrawDoubleTokensAction action)
        {
            return ReduceGeneralTokenAction(state, action.Tokens.ToArray());
        }

        public GameState Reduce(GameState state, ReturnTokensAction action)
        {
            var invertedValues = action.Tokens.ToArray().Select(t => t * -1);

            return ReduceGeneralTokenAction(state, invertedValues.ToArray());
        }
    }
}