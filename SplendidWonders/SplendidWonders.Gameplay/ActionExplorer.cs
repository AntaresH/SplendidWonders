using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using System.Collections.Immutable;

namespace SplendidWonders.Gameplay
{
    public class ActionExplorer
    {
        public static List<GameAction>? GetValidActions(GameState gameState)
        {
            var validActions = new List<GameAction>();

            var returnTokensActions = ReturnTokensExplorer.GetActions(gameState);
            var selectLeaderActions = SelectLeaderExplorer.GetActions(gameState);

            if (returnTokensActions.Any())
            {
                validActions.AddRange(returnTokensActions);
                return validActions;
            }
            else if (selectLeaderActions.Any())
            {
                validActions.AddRange(selectLeaderActions);
                return validActions;
            }
            else
            {
                validActions.AddRange(DrawThreeTokensExplorer.GetActions(gameState));
                validActions.AddRange(DrawDoubleTokensExplorer.GetActions(gameState));
                validActions.AddRange(ReserveFromBoardExplorer.GetActions(gameState));
                validActions.AddRange(ReserveFromDeckExplorer.GetActions(gameState));
                validActions.AddRange(PurchaseExplorer.GetActions(gameState));
            }

            return validActions;
        }




    }
}