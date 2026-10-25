using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Reducers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace SplendidWonders.Gameplay
{
    public class GameInstance
    {
        public Guid Id { get; internal set; }
        public DateTime StartedAt { get; internal set; }
        public bool IsGameOver { get; internal set; }

        public GameState GameState { get; internal set; }

        private List<GameAction> _validActions;
        public List<GameAction> ValidActions
        {
            get
            {
                if (_validActions == null)
                {
                    _validActions = ActionExplorer.GetValidActions(GameState);
                }

                return _validActions;
            }
        }

        private GameConfiguration _gameConfiguration;

        public GameInstance(GameConfiguration gameConfiguration)
        {
            Id = Guid.NewGuid();
            StartedAt = DateTime.UtcNow;
            IsGameOver = false;

            _gameConfiguration = gameConfiguration;
        }

        public void Start()
        {
            // Initialize game state, players, and other necessary components
            Console.WriteLine("Starting the game...");

            GameState = GameFactory.GetNewGameState(_gameConfiguration);
        }

        public Dictionary<string, int> GetFinalScores()
        {
            throw new NotImplementedException();
        }

        public bool ValidateCommand(GameAction command)
        {
            return ValidActions.Any(action => action.Equals(command));
        }

        public void ExecuteCommand(GameAction command)
        {
            switch (command)
            {
                case DrawThreeTokensAction a:
                    DrawThreeTokensReducer.Reduce(GameState, a);
                    break;
                case DrawDoubleTokensAction a:
                    DrawDoubleTokensReducer.Reduce(GameState, a);
                    break;
                case ReturnTokensAction a:
                    ReturnTokensReducer.Reduce(GameState, a);
                    break;
                case PurchaseAction a:
                    PurchaseReducer.Reduce(GameState, a);
                    break;
                case ReserveFromBoardAction a:
                    ReserveFromBoardReducer.Reduce(GameState, a);
                    break;
                case ReserveFromDeckAction a:
                    ReserveFromDeckReducer.Reduce(GameState, a);
                    break;
                case SelectLeaderAction a:
                    SelectLeaderReducer.Reduce(GameState, a);
                    break;
                default:
                    throw new InvalidOperationException("This action is not supported... yet");
            }
        }
    }
}
