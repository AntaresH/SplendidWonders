using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReserveActionReducer : IStateReducer<ReserveFromBoardAction>, IStateReducer<ReserveFromDeckAction>
    {
        public GameState Reduce(GameState baseState, ReserveFromBoardAction action)
        {
            var currentPlayerId = baseState.CurrentPlayer.Id;
            var reducedStateBuilder = new GameStateBuilder(baseState);

            SetPlayerReservedCards(baseState, action.Card, currentPlayerId, reducedStateBuilder);
            SetPolymaths(baseState, currentPlayerId, reducedStateBuilder);
            SetReplacementCard(baseState, action.Card, reducedStateBuilder);
            SetActivePlayer(baseState, reducedStateBuilder);

            return reducedStateBuilder.Build();
        }

        public GameState Reduce(GameState baseState, ReserveFromDeckAction action)
        {
            var currentPlayerId = baseState.CurrentPlayer.Id;
            var reducedStateBuilder = new GameStateBuilder(baseState);

            SetPlayerReservedCards(baseState, baseState[action.Level].Deck[0], currentPlayerId, reducedStateBuilder);
            SetPolymaths(baseState, currentPlayerId, reducedStateBuilder);
            SetShorterDeck(baseState, action.Level, reducedStateBuilder);
            SetActivePlayer(baseState, reducedStateBuilder);

            return reducedStateBuilder.Build();
        }

        private void SetShorterDeck(GameState baseState, int level, GameStateBuilder reducedStateBuilder)
        {
            reducedStateBuilder.WithLevel(
                level, 
                new CardGroup() { 
                    Market = baseState[level].Market, 
                    Deck = baseState[level].Deck.RemoveAt(0) 
                });
        }

        private static void SetPlayerReservedCards(GameState baseState, DevelopmentCard card, int currentPlayerId, GameStateBuilder reducedStateBuilder)
        {
            var playerReserved = baseState.CurrentPlayer.ReservedCards;
            playerReserved = playerReserved.Add(card);

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

        private void SetActivePlayer(GameState baseState, GameStateBuilder reducedStateBuilder)
        {
            var playerStore = baseState.CurrentPlayer.Store;

            int allPlayerTokens = playerStore.Yellow + playerStore.Blue + playerStore.Green + playerStore.Brown + playerStore.Red + playerStore.Polymath;
            int currentPlayerId = allPlayerTokens <= 10
                ? baseState.CurrentPlayer.Id + 1 % baseState.Players.Count
                : baseState.CurrentPlayer.Id;

            reducedStateBuilder.WithCurrentPlayer(currentPlayerId);
        }

        private static void SetReplacementCard(GameState baseState, DevelopmentCard card, GameStateBuilder reducedStateBuilder)
        {
            int level = card.Level;

            var cardGroup = baseState[level];
            int index = cardGroup.Market.IndexOf(card);

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