using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay;
using Resources = SplendidWonders.CLI.Properties.Resources;
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
            Console.WriteLine(Resources.UserAborted);
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(Resources.ErrorDuringGameFormat, ex.Message));
        }
    }

    private void ExecuteTurn()
    {
        while (true)
        {
            _gameView.DisplayGame(_game.GameState, _game.ValidActions);

            var command = _commandHandler.GetCommand(_game.GameState, _game.ValidActions);

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