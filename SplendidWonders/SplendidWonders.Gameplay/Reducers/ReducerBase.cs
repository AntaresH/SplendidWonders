using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReducerBase
    {
        protected GameState ResolveMarket(GameState baseState, CardAction action)
        {
            int level = action.Card.Level;

            var cardGroup = baseState[level];

            if (!cardGroup.Market.Contains(action.Card))
            {
                return baseState;
            }

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

            return baseState.UpdateLevel(level, new CardGroup() { Deck = newDeck, Market = newMarket });
        }

        protected GameState ResolveActivePlayer(GameState baseState)
        {
            bool tokenOverflow = CheckTokenOverflow(baseState);
            bool multipleLeadersEligible = CheckMultipleLeadersEligible(baseState);

            bool advanceToNextPlayer = !tokenOverflow && !multipleLeadersEligible;

            int nextPlayerId = advanceToNextPlayer
                ? baseState.ActivePlayer.Id % baseState.Players.Count + 1
                : baseState.ActivePlayer.Id;

            return baseState with { ActivePlayer = baseState.Players[nextPlayerId - 1] };
        }

        private bool CheckMultipleLeadersEligible(GameState baseState)
        {
            return GetEligibleLeaders(baseState).Count() > 1;
        }

        private bool CheckTokenOverflow(GameState state)
        {
            var playerStore = state.ActivePlayer.Store;
            int allPlayerTokens = playerStore.Yellow + playerStore.Blue + playerStore.Green + playerStore.Brown + playerStore.Red + playerStore.Polymath;

            return allPlayerTokens > 10;
        }

        protected IEnumerable<Leader> GetEligibleLeaders(GameState state)
        {
            return state.Leaders.Where(leader => CheckLeaderEligibility(state, leader));
        }

        private bool CheckLeaderEligibility(GameState state, Leader leader)
        {
            bool passRequirements = true;

            int[] leaderReqsArray = leader.Requirements.ToArray();
            int[] playerTableauArray =
            [
                state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentBonus.Yellow),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentBonus.Blue),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentBonus.Green),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentBonus.Brown),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentBonus.Red),
                ];

            for (int i = 0; i < 5; i++)
            {
                if (leaderReqsArray[i] > playerTableauArray[i])
                {
                    passRequirements = false;
                    break;
                }
            }

            return passRequirements;
        }
    }
}
