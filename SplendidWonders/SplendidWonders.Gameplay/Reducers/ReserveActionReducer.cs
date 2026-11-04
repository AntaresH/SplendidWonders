using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReserveActionReducer : ReducerBase, IStateReducer<ReserveFromBoardAction>, IStateReducer<ReserveFromDeckAction>
    {
        public GameState Reduce(GameState baseState, ReserveFromBoardAction action)
        {
            return baseState
                .Transform(action, ResolveBoardReserve)
                .Transform(action, ResolveReplacement)
                .Transform(ResolveActivePlayer);
        }

        public GameState Reduce(GameState baseState, ReserveFromDeckAction action)
        {
            return baseState
                .Transform(action, ResolveDeckReserve)
                .Transform(action, ResolveDeckDraw)
                .Transform(ResolveActivePlayer);
        }

        private GameState ResolveDeckDraw(GameState baseState, ReserveFromDeckAction action)
        {
            return baseState.UpdateLevel(action.Level, new CardGroup() { Deck = baseState[action.Level].Deck.RemoveAt(0), Market = baseState[action.Level].Market });
        }

        private GameState ResolveBoardReserve(GameState baseState, ReserveFromBoardAction action)
        {
            return ResolveCardReserve(baseState, action.Card);
        }

        private GameState ResolveDeckReserve(GameState baseState, ReserveFromDeckAction action)
        {
            var card = baseState[action.Level].Deck.First();

            return ResolveCardReserve(baseState, card);
        }

        private static GameState ResolveCardReserve(GameState baseState, DevelopmentCard card)
        {
            var playerStore = baseState.ActivePlayer.Store;
            var tokenSupply = baseState.TokenSupply;

            if (tokenSupply.Polymath > 0)
            {
                playerStore.Polymath++;
                tokenSupply.Polymath--;
            }

            var playerReserved = baseState.ActivePlayer.ReservedCards;
            playerReserved = playerReserved.Add(card);

            return baseState.UpdatePlayerData(baseState.ActivePlayer.Id, playerStore, playerReserved, baseState.ActivePlayer.Tableau, baseState.ActivePlayer.AcquiredLeaders)
                            .UpdateTokenSupply(tokenSupply);
        }
    }
}