using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class PurchaseReducer : ReducerBase, IStateReducer<PurchaseAction>
    {
        public GameState Reduce(GameState baseState, PurchaseAction action)
        {
            return baseState
                .Transform(action, ResolvePurchase)
                .Transform(action, ResolveReplacement)
                .Transform(ResolveLeaders)
                .Transform(ResolveGameEndTrigger)
                .Transform(ResolveGameProgression);
        }

        private GameState ResolvePurchase(GameState baseState, PurchaseAction action)
        {
            var cardCostArray = action.Card.Cost.ToArray();
            var tokenSupplyArray = baseState.TokenSupply.ToArray();
            var playerStoreArray = baseState.ActivePlayer.Store.ToArray();

            int[] playerBonuses =
            [
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Yellow),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Blue),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Green),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Brown),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Red),
            ];

            int totalUsedPolymaths = 0;
            for (int i = 0; i < 5; i++)
            {
                int colorUsedPolymaths = Math.Max(0, cardCostArray[i] - playerBonuses[i] - playerStoreArray[i]);

                tokenSupplyArray[i] += Math.Max(0, cardCostArray[i] - playerBonuses[i] - colorUsedPolymaths);
                playerStoreArray[i] -= Math.Max(0, cardCostArray[i] - playerBonuses[i] - colorUsedPolymaths);

                totalUsedPolymaths += colorUsedPolymaths;
            }

            tokenSupplyArray[5] += totalUsedPolymaths;
            playerStoreArray[5] -= totalUsedPolymaths;

            var playerTableau = baseState.ActivePlayer.Tableau.Add(action.Card);
            var playerReserved = baseState.ActivePlayer.ReservedCards;
            var playerStore = TokenGroup.FromArray(playerStoreArray);

            if (baseState.ActivePlayer.ReservedCards.Contains(action.Card))
            {
                playerReserved = baseState.ActivePlayer.ReservedCards.Remove(action.Card);
            }

            return baseState
                .UpdatePlayerData(baseState.ActivePlayer.Id, playerStore, playerReserved, playerTableau, baseState.ActivePlayer.AcquiredLeaders)
                .UpdateTokenSupply(TokenGroup.FromArray(tokenSupplyArray));
        }

        private GameState ResolveLeaders(GameState baseState)
        {
            var eligibleLeaders = GetEligibleLeaders(baseState);

            if (eligibleLeaders.Count() == 1)
            {
                return AcquireLeader(baseState, eligibleLeaders.First());
            }

            return baseState;
        }
    }
}