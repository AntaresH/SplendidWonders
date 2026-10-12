using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.View;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay;
using System.Text.RegularExpressions;

namespace SplendidWonders.CLI;

internal class GameExecutor
{
    private GameInstance _game;
    private IGameView _gameView;
    private IGameInput _commandHandler;

    internal GameExecutor(bool useExtendedLeaders, List<string> players, IGameView gameView, IGameInput gameInput)
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
            Console.WriteLine($"A felhasználó megszakította a játékot!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba történt a játék során: {ex.Message}");
        }
    }

    private void ExecuteTurn()
    {
        _gameView.DisplayGameState(_game.GameState);

        var validActions = _game.ValidActions;
        _gameView.DisplayValidActions(validActions);

        var command = _commandHandler.GetCommand(validActions, _game.GameState);
        _game.HandleCommand(command);
    }
}