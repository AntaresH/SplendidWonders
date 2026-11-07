using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using SplendidWonders.Common;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using System.Text.RegularExpressions;

namespace SplendidWonders.CLI;

internal class GameOrchestrator
{
    private GameExecution _game;
    private IGameView _gameView;
    private IGameInput _commandHandler;

    public GameOrchestrator(GameConfiguration gameConfiguration, IGameView gameView, IGameInput gameInput, IActionExplorer actionExplorer, IStateReducer actionReducer)
    {
        _game = new GameExecution(gameConfiguration, actionExplorer, actionReducer);
        _gameView = gameView;
        _commandHandler = gameInput;
    }

    public void Start()
    {
        try
        {
            _game.Start();

            while (_game.GameState.Progression == GameProgression.InProgress)
            {
                HandleCommand();
            }

            _gameView.DisplayBoard(_game.GameState);
            _gameView.DisplayGameOver(_game.GetFinalStandings());
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
        string invalidCommandMsg = null;

        while (true)
        {
            _gameView.DisplayGame(_game.GameState, _game.ValidActions, invalidCommandMsg);
            invalidCommandMsg = null;

            var command = _commandHandler.ReadCommand(_game.GameState, _game.ValidActions);

            if (command is PresentationCommand)
            {
                _gameView.DisplayPresentationCommand(command, _game.GameState);
                continue;
            }
            else if (command is GameCommand gameCommand)
            {
                if (_game.ValidateCommand(gameCommand.Action))
                {
                    _game.ExecuteCommand(gameCommand.Action);
                    return;
                }
                else
                {
                    invalidCommandMsg = string.Format(Strings.InvalidCommandTypeStateFormat, command.CommandText);
                }
            }
        }
    }
}