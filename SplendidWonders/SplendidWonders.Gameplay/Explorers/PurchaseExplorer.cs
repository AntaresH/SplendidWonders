using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Explorers
{
    public class PurchaseExplorer
    {
        public static IEnumerable<GameAction> GetActions(GameState gameState)
        {
            List<PurchaseAction> purchaseActions = new List<PurchaseAction>();

            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    var card = gameState[i][j];

                    if (card == null || card == Cards.Empty)
                        continue;

                    if (CalculatePurchasability(card.Cost, gameState.CurrentPlayer.Store, gameState.CurrentPlayer.Tableau))
                    {
                        purchaseActions.Add(new PurchaseAction() { Card = card });
                    }
                }
            }

            foreach (var card in gameState.CurrentPlayer.ReservedCards)
            {
                if (CalculatePurchasability(card.Cost, gameState.CurrentPlayer.Store, gameState.CurrentPlayer.Tableau))
                {
                    purchaseActions.Add(new PurchaseAction() { Card = card });
                }
            }

            return purchaseActions;
        }

        private static bool CalculatePurchasability(TokenGroup cardCosts, TokenGroup playerStore, ImmutableList<DevelopmentCard> playerTableau)
        {
            int playerYellowBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Yellow);
            int playerBlueBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Blue);
            int playerGreenBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Green);
            int playerBrownBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Brown);
            int playerRedBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Red);


            int deficit =
                Math.Min(0, playerYellowBonuses + playerStore.Yellow - cardCosts.Yellow) +
                Math.Min(0, playerBlueBonuses + playerStore.Blue - cardCosts.Blue) +
                Math.Min(0, playerGreenBonuses + playerStore.Green - cardCosts.Green) +
                Math.Min(0, playerBrownBonuses + playerStore.Brown - cardCosts.Brown) +
                Math.Min(0, playerRedBonuses + playerStore.Red - cardCosts.Red);

            int availablePolymaths = playerStore.Polymath - cardCosts.Polymath;

            return availablePolymaths >= 0 && deficit + availablePolymaths >= 0;
        }
    }
}
