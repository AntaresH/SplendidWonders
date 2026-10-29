using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace SplendidWonders.Gameplay
{
    public class GameExecution
    {
        public Guid Id { get; internal set; }
        public DateTime StartedAt { get; internal set; }
        public bool IsGameOver { get; internal set; }

        public GameState GameState { get; internal set; }

        public IList<GameAction> ValidActions
        {
            get
            {
                if (field == null || !field.Any())
                {
                    field = _actionExplorer.Explore(GameState);
                }

                return field;
            }
        }

        private readonly GameConfiguration _gameConfiguration;
        private readonly IActionExplorer _actionExplorer;
        private readonly IStateReducer _stateReducer;

        public GameExecution(GameConfiguration gameConfiguration, IActionExplorer actionExplorer, IStateReducer stateReducer)
        {
            Id = Guid.NewGuid();
            StartedAt = DateTime.UtcNow;
            IsGameOver = false;

            _gameConfiguration = gameConfiguration;
            _actionExplorer = actionExplorer;
            _stateReducer = stateReducer;
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

            GameState = _stateReducer.Reduce(GameState, command);
            ValidActions.Clear();
        }
    }
}
