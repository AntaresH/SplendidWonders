using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay;
using System.Text.RegularExpressions;

namespace SplendidWonders.CLI;

internal class GameOrchestrator
{
    private GameInstance _game;
    private IGameView _gameView;
    private IGameInput _commandHandler;

    internal GameOrchestrator(bool useExtendedLeaders, List<string> players, IGameView gameView, IGameInput gameInput)
    {
        _game = new GameInstance(useExtendedLeaders ? GameConfiguration.ExtendedLeaders : GameConfiguration.Standard, players);
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
                ExecuteTurn();
            }

            _gameView.DisplayGameOver(_game.GetFinalScores());
        }
        catch (UserAbortException ex)
        {
            Console.WriteLine(SplendidWonders.CLI.Properties.Resources.UserAborted);
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(SplendidWonders.CLI.Properties.Resources.ErrorDuringGameFormat, ex.Message));
        }
    }

    private void ExecuteTurn()
    {
        while (true)
        {
            _gameView.DisplayGameState(_game.GameState);

            var validActions = _game.ValidActions;
            _gameView.DisplayValidActions(validActions);

            var command = _commandHandler.GetCommand(validActions, _game.GameState);

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