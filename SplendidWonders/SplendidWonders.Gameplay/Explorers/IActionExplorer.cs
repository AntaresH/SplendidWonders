using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Explorers
{
    public interface IActionExplorer
    {
        IList<GameAction> Explore(GameState state);
    }

    public interface IActionExplorer<T> where T : GameAction
    {
        IEnumerable<T> Explore(GameState state);
    }
}