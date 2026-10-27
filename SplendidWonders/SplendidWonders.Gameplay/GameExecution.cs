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
    public class GameExecution
    {
        public Guid Id { get; internal set; }
        public DateTime StartedAt { get; internal set; }
        public bool IsGameOver { get; internal set; }

        public GameState GameState { get; internal set; }

        public List<GameAction> ValidActions
        {
            get
            {
                if (field == null || !field.Any())
                {
                    field = ActionExplorer.GetValidActions(GameState);
                }

                return field;
            }
        }

        private GameConfiguration _gameConfiguration;

        public GameExecution(GameConfiguration gameConfiguration)
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
            GameState = ActionReducer.Reduce(GameState, command);
            ValidActions.Clear();
        }
    }
}
