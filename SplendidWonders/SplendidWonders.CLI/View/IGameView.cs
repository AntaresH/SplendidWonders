using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.View
{
    public interface IGameView
    {
        void DisplayGameOver(Dictionary<string, int> value);
        void DisplayGameState(GameState gameState);
        void DisplayValidActions(List<GameAction> validActions);
        void DisplayTurnStart(Player currentPlayer);
        void DisplayInvalidCommandMessage();
        void DisplayCommandCategoryRequest(List<string> validActionCategories);
        void DisplayThreeTokensArgumentRequest();
        void DisplayDoubleTokensArgumentRequest();
        void DisplayPurchaseArgumentRequest();
        void DisplayReserveFromBoardArgumentRequest();
        void DisplayReserveFromDeckArgumentRequest();

    }
}
