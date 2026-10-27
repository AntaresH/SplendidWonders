using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Explorers
{
    public class ReserveFromDeckExplorer
    {
        public static IEnumerable<GameAction> GetActions(GameState gameState)
        {
            List<ReserveFromDeckAction> reserveFromDeckActions = new List<ReserveFromDeckAction>();

            if (gameState.CurrentPlayer.ReservedCards.Count < 3)
            {
                for (int i = 1; i < 4; i++)
                {
                    var deck = gameState[i];

                    if (deck.Remaining > 0)
                    {
                        reserveFromDeckActions.Add(new ReserveFromDeckAction() { Level = i });
                    }
                }
            }

            return reserveFromDeckActions;
        }
    }
}
