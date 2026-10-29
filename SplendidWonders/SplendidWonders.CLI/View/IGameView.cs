using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.View
{
    public interface IGameView
    {
        void DisplayGameOver(Dictionary<string, int> value);
        void DisplayGame(GameState gameState, IEnumerable<GameAction> validActions);
        void DisplayValidActions(IEnumerable<GameAction> validActions);
        void DisplayTurnStart(Player currentPlayer);
        void DisplayInvalidCommandMessage();
        void DisplayThreeTokensArgumentRequest();
        void DisplayDoubleTokensArgumentRequest();
        void DisplayPurchaseArgumentRequest();
        void DisplayReserveFromBoardArgumentRequest();
        void DisplayReserveFromDeckArgumentRequest();
        void DisplayInvalidCommand(GameAction command);
        void DisplayGameTitle();
    }
}
