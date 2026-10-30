using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReserveFromBoardReducer : IStateReducer<ReserveFromBoardAction>
    {
        public GameState Reduce(GameState baseState, ReserveFromBoardAction action)
        {
            var currentPlayerId = baseState.CurrentPlayer.Id;
            var reducedStateBuilder = new GameStateBuilder(baseState);

            SetPlayerReservedCards(baseState, action, currentPlayerId, reducedStateBuilder);
            SetPolymaths(baseState, currentPlayerId, reducedStateBuilder);
            SetCurrentPlayer(baseState, reducedStateBuilder);
            SetReplacementCard(baseState, action, reducedStateBuilder);

            return reducedStateBuilder.Build();
        }

        private static void SetPlayerReservedCards(GameState baseState, ReserveFromBoardAction action, int currentPlayerId, GameStateBuilder reducedStateBuilder)
        {
            var playerReserved = baseState.CurrentPlayer.ReservedCards;
            playerReserved = playerReserved.Add(action.Card);

            reducedStateBuilder.WithPlayerReservedCards(currentPlayerId, playerReserved);
        }

        private static void SetPolymaths(GameState baseState, int currentPlayerId, GameStateBuilder reducedStateBuilder)
        {
            if (baseState.TokenSupply.Polymath > 0)
            {
                var playerStore = baseState.CurrentPlayer.Store;
                var tokenSupply = baseState.TokenSupply;

                playerStore.Polymath++;
                tokenSupply.Polymath--;

                reducedStateBuilder.WithPlayerStore(currentPlayerId, playerStore);
                reducedStateBuilder.WithTokenSupply(tokenSupply);
            }
        }

        private void SetCurrentPlayer(GameState baseState, GameStateBuilder reducedStateBuilder)
        {
            var playerStore = baseState.CurrentPlayer.Store;

            int allPlayerTokens = playerStore.Yellow + playerStore.Blue + playerStore.Green + playerStore.Brown + playerStore.Red + playerStore.Polymath;
            int currentPlayerId = allPlayerTokens <= 10
                ? baseState.CurrentPlayer.Id + 1 % baseState.Players.Count
                : baseState.CurrentPlayer.Id;

            reducedStateBuilder.WithCurrentPlayer(currentPlayerId);
        }

        private static void SetReplacementCard(GameState baseState, ReserveFromBoardAction action, GameStateBuilder reducedStateBuilder)
        {
            int level = action.Card.Level;

            var cardGroup = baseState[level];
            int index = cardGroup.Market.IndexOf(action.Card);

            var newMarket = cardGroup.Market.RemoveAt(index);
            var newDeck = cardGroup.Deck;

            if (cardGroup.Deck.Any())
            {
                newMarket = newMarket.Insert(index, cardGroup.Deck[0]);
                newDeck = newDeck.RemoveAt(0);
            }
            else
            {
                newMarket = newMarket.Insert(index, Cards.Empty);
            }

            reducedStateBuilder.WithLevel(level, new CardGroup() { Deck = newDeck, Market = newMarket });
        }
    }
}