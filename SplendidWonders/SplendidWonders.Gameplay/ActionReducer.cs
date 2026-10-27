using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Reducers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay
{
    public class ActionReducer
    {
        internal static GameState Reduce(GameState gameState, GameAction command)
        {
            switch (command)
            {
                case DrawThreeTokensAction a:
                    return DrawThreeTokensReducer.Reduce(gameState, a);
                case DrawDoubleTokensAction a:
                    return DrawDoubleTokensReducer.Reduce(gameState, a);
                case ReturnTokensAction a:
                    return ReturnTokensReducer.Reduce(gameState, a);
                case PurchaseAction a:
                    return PurchaseReducer.Reduce(gameState, a);
                case ReserveFromBoardAction a:
                    return ReserveFromBoardReducer.Reduce(gameState, a);
                case ReserveFromDeckAction a:
                    return ReserveFromDeckReducer.Reduce(gameState, a);
                case SelectLeaderAction a:
                    return SelectLeaderReducer.Reduce(gameState, a);
                default:
                    throw new InvalidOperationException("This action is not supported... yet");
            }
        }
    }
}
