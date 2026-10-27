using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Explorers
{
    public class SelectLeaderExplorer
    {
        public static IEnumerable<GameAction> GetActions(GameState gameState)
        {
            List<SelectLeaderAction> selectLeaderActions = new List<SelectLeaderAction>();

            foreach (var leader in gameState.Leaders)
            {
                Dictionary<DevelopmentBonus, int> requirements = new Dictionary<DevelopmentBonus, int>()
                {
                    { DevelopmentBonus.Yellow, leader.Requirements.Yellow },
                    { DevelopmentBonus.Blue, leader.Requirements.Blue },
                    { DevelopmentBonus.Green, leader.Requirements.Green },
                    { DevelopmentBonus.Brown, leader.Requirements.Brown },
                    { DevelopmentBonus.Red, leader.Requirements.Red },
                };

                bool hasFailedRequirement = false;

                foreach (var requirement in requirements)
                {
                    if (requirement.Value == 0)
                        continue;

                    if (gameState.CurrentPlayer.Tableau.Count(card => card.Bonus == requirement.Key) < requirement.Value)
                    {
                        hasFailedRequirement = true;
                        break;
                    }
                }

                if (!hasFailedRequirement)
                {
                    selectLeaderActions.Add(new SelectLeaderAction() { Leader = leader });
                }
            }

            return selectLeaderActions;
        }
    }
}
