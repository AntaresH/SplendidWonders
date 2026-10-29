using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using System.Collections.Immutable;

namespace SplendidWonders.Gameplay
{
    public class ActionExplorer : IActionExplorer
    {
        private readonly IActionExplorer<DrawThreeTokensAction> _drawThreeTokensExplorer;
        private readonly IActionExplorer<DrawDoubleTokensAction> _drawDoubleTokensExplorer;
        private readonly IActionExplorer<ReserveFromBoardAction> _reserveFromBoardExplorer;
        private readonly IActionExplorer<ReserveFromDeckAction> _reserveFromDeckExplorer;
        private readonly IActionExplorer<PurchaseAction> _purchaseExplorer;
        private readonly IActionExplorer<SelectLeaderAction> _selectLeaderExplorer;
        private readonly IActionExplorer<ReturnTokensAction> _returnTokensExplorer;

        public ActionExplorer(IActionExplorer<DrawThreeTokensAction> drawThreeTokensExplorer,
            IActionExplorer<DrawDoubleTokensAction> drawDoubleTokensExplorer, 
            IActionExplorer<ReturnTokensAction> returnTokensExplorer, 
            IActionExplorer<ReserveFromBoardAction> reserveFromBoardExplorer, 
            IActionExplorer<ReserveFromDeckAction> reserveFromDeckExplorer,
            IActionExplorer<PurchaseAction> purchaseExplorer,
            IActionExplorer<SelectLeaderAction> selectLeaderExplorer
            )
        {
            _drawThreeTokensExplorer = drawThreeTokensExplorer;
            _drawDoubleTokensExplorer = drawDoubleTokensExplorer;
            _reserveFromBoardExplorer = reserveFromBoardExplorer;
            _reserveFromDeckExplorer = reserveFromDeckExplorer;
            _purchaseExplorer = purchaseExplorer;
            _selectLeaderExplorer = selectLeaderExplorer;
            _returnTokensExplorer = returnTokensExplorer;
        }

        public IList<GameAction> Explore(GameState state)
        {
            var validActions = new List<GameAction>();

            var returnTokensActions = _returnTokensExplorer.Explore(state);
            var selectLeaderActions = _selectLeaderExplorer.Explore(state);

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
                validActions.AddRange(_drawThreeTokensExplorer.Explore(state));
                validActions.AddRange(_drawDoubleTokensExplorer.Explore(state));
                validActions.AddRange(_reserveFromBoardExplorer.Explore(state));
                validActions.AddRange(_reserveFromDeckExplorer.Explore(state));
                validActions.AddRange(_purchaseExplorer.Explore(state));
            }

            return validActions;
        }
    }
}