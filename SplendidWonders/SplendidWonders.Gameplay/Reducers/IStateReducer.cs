using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public interface IStateReducer
    {
        GameState Reduce(GameState state, GameAction action);
    }

    public interface IStateReducer<T> where T : GameAction
    {
        GameState Reduce(GameState state, T action);
    }
}