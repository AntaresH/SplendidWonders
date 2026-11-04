using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay
{
    public class StateReducer : IStateReducer
    {
        private readonly IStateReducer<DrawThreeTokensAction> _drawThreeTokensReducer;
        private readonly IStateReducer<DrawDoubleTokensAction> _drawDoubleTokensReducer;
        private readonly IStateReducer<ReserveFromBoardAction> _reserveFromBoardReducer;
        private readonly IStateReducer<ReserveFromDeckAction> _reserveFromDeckReducer;
        private readonly IStateReducer<PurchaseAction> _purchaseReducer;
        private readonly IStateReducer<SelectLeaderAction> _selectLeaderReducer;
        private readonly IStateReducer<ReturnTokensAction> _returnTokensReducer;

        public StateReducer(IStateReducer<DrawThreeTokensAction> drawThreeTokensReducer,
            IStateReducer<DrawDoubleTokensAction> drawDoubleTokensReducer,
            IStateReducer<ReturnTokensAction> returnTokensReducer,
            IStateReducer<ReserveFromBoardAction> reserveFromBoardReducer,
            IStateReducer<ReserveFromDeckAction> reserveFromDeckReducer,
            IStateReducer<PurchaseAction> purchaseReducer,
            IStateReducer<SelectLeaderAction> selectLeaderReducer
            )
        {
            _drawThreeTokensReducer = drawThreeTokensReducer;
            _drawDoubleTokensReducer = drawDoubleTokensReducer;
            _reserveFromBoardReducer = reserveFromBoardReducer;
            _reserveFromDeckReducer = reserveFromDeckReducer;
            _purchaseReducer = purchaseReducer;
            _selectLeaderReducer = selectLeaderReducer;
            _returnTokensReducer = returnTokensReducer;
        }

        public GameState Reduce(GameState gameState, GameAction command)
        {
            return command switch
            {
                DrawThreeTokensAction a => _drawThreeTokensReducer.Reduce(gameState, a),
                DrawDoubleTokensAction a => _drawDoubleTokensReducer.Reduce(gameState, a),
                ReturnTokensAction a => _returnTokensReducer.Reduce(gameState, a),
                PurchaseAction a => _purchaseReducer.Reduce(gameState, a),
                ReserveFromBoardAction a => _reserveFromBoardReducer.Reduce(gameState, a),
                ReserveFromDeckAction a => _reserveFromDeckReducer.Reduce(gameState, a),
                SelectLeaderAction a => _selectLeaderReducer.Reduce(gameState, a),
                _ => throw new InvalidOperationException("This action is not supported... yet"),
            };
        }
    }
}
