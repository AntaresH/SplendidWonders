using SplendidWonders.CLI.Input;
using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.CLI.View
{
    public interface IGameView
    {
        void DisplayGameOver(List<Player> players);
        void DisplayGame(GameState gameState, IEnumerable<GameAction> validActions, string previousInvalidCommand);
        void DisplayValidActions(IEnumerable<GameAction> validActions);
        void DisplayTurnStart(Player currentPlayer);
        void DisplayInvalidCommandMessage();
        void DisplayThreeTokensArgumentRequest();
        void DisplayDoubleTokensArgumentRequest();
        void DisplayPurchaseFromBoardArgumentRequest(IEnumerable<DevelopmentCard> purchasableCards);
        void DisplayPurchaseFromReservedArgumentRequest(ImmutableList<DevelopmentCard> reservedCards);
        void DisplayReserveFromBoardArgumentRequest();
        void DisplayReserveFromDeckArgumentRequest();
        void DisplayInvalidCommand(GameAction command);
        void DisplayGameTitle();
        void DisplayReturnTokensArgumentRequest();
        void DisplayPresentationCommand(ApplicationCommand command, GameState gameState);
        void DisplaySelectLeaderArgumentRequest(IEnumerable<Leader> eligibleLeaders);
        void DisplayBoard(GameState gameState);
    }
}
