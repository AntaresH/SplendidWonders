using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class TokenActionReducer : ReducerBase, IStateReducer<DrawDoubleTokensAction>, IStateReducer<DrawThreeTokensAction>, IStateReducer<ReturnTokensAction>
    {
        public GameState Reduce(GameState baseState, DrawThreeTokensAction action)
        {
            return ReduceGeneralTokenAction(baseState, action.Tokens.ToArray());
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

        private GameState ReduceGeneralTokenAction(GameState baseState, int[] selectedTokens)
        {
            return baseState
                .Transform(selectedTokens, MoveTokens)
                .Transform(ResolveGameProgression);
        }

        private GameState MoveTokens(GameState baseState, int[] selectedTokens)
        {
            var playerStore = baseState.ActivePlayer.Store.ToArray();
            var tokenSupply = baseState.TokenSupply.ToArray();

            for (int i = 0; i < 5; i++)
            {
                playerStore[i] += selectedTokens[i];
                tokenSupply[i] -= selectedTokens[i];
            }

            return baseState.UpdatePlayerData(baseState.ActivePlayer.Id, TokenGroup.FromArray(playerStore), baseState.ActivePlayer.ReservedCards, baseState.ActivePlayer.Tableau, baseState.ActivePlayer.AcquiredLeaders)
                            .UpdateTokenSupply(TokenGroup.FromArray(tokenSupply));
        }
    }
}