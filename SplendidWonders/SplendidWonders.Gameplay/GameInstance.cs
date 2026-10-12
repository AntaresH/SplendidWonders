using SplendidWonders.Core.Setup;
using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SplendidWonders.Gameplay
{
    public class GameInstance
    {
        public Guid Id { get; internal set; }
        public DateTime StartedAt { get; internal set; }
        public bool IsGameOver { get; internal set; }

        public GameState GameState { get; internal set; }

        public List<GameAction> ValidActions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private GameConfiguration _gameConfiguration;

        private List<string> _playerNames;

        public GameInstance(GameConfiguration gameConfiguration, List<string> playerNames)
        {
            Id = Guid.NewGuid();
            StartedAt = DateTime.UtcNow;
            IsGameOver = false;

            _gameConfiguration = gameConfiguration;
            _playerNames = playerNames;
        }

        public void Start()
        {
            // Initialize game state, players, and other necessary components
            Console.WriteLine("Starting the game...");

            GameState = GameFactory.GetNewGameState(_gameConfiguration, _playerNames);
        }

        public Dictionary<string, int> GetFinalScores()
        {
            throw new NotImplementedException();
        }

        public bool ValidateCommand(GameAction command)
        {
            throw new NotImplementedException();
        }

        public void HandleCommand(GameAction command)
        {
            throw new NotImplementedException();
        }
    }
}
