using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Explorers
{
    public class ReserveFromBoardExplorer : IActionExplorer<ReserveFromBoardAction>
    {
        public IEnumerable<ReserveFromBoardAction> Explore(GameState gameState)
        {
            List<ReserveFromBoardAction> reserveFromBoardActions = new List<ReserveFromBoardAction>();

            if (gameState.CurrentPlayer.ReservedCards.Count < 3)
            {
                for (int i = 1; i < 4; i++)
                {
                    var cardList = gameState[i];

                    for (int j = 1; j < 5; j++)
                    {
                        var card = cardList[j];

                        if (card != Cards.Empty)
                        {
                            reserveFromBoardActions.Add(new ReserveFromBoardAction() { Card = card });
                        }
                    }
                }
            }

            return reserveFromBoardActions;
        }
    }
}
