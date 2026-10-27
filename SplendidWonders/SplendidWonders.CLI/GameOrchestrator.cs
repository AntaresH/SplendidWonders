using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using SplendidWonders.Common;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay;
using System.Text.RegularExpressions;

namespace SplendidWonders.CLI;

internal class GameOrchestrator
{
    private GameExecution _game;
    private IGameView _gameView;
    private IGameInput _commandHandler;

    internal GameOrchestrator(GameConfiguration gameConfiguration, IGameView gameView, IGameInput gameInput)
    {
        _game = new GameExecution(gameConfiguration);
        _gameView = gameView;
        _commandHandler = gameInput;
    }

    internal void Start()
    {
        try
        {
            _game.Start();

            while (!_game.IsGameOver)
            {
                _gameView.DisplayTurnStart(_game.GameState.CurrentPlayer);
                HandleCommand();
            }

            _gameView.DisplayGameOver(_game.GetFinalScores());
        }
        catch (UserAbortException ex)
        {
            Console.WriteLine(Strings.UserAborted);
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(Strings.ErrorDuringGameFormat, ex.Message));
        }
    }

    private void HandleCommand()
    {
        while (true)
        {
            _gameView.DisplayGame(_game.GameState, _game.ValidActions);

            var command = _commandHandler.ReadCommand(_game.GameState, _game.ValidActions);

            if (_game.ValidateCommand(command))
            {
                _game.ExecuteCommand(command);
                return;
            }
            else
            {
                _gameView.DisplayInvalidCommand(command);
            }
        }
    }
}