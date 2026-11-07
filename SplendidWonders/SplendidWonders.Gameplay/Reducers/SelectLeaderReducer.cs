using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class SelectLeaderReducer : ReducerBase, IStateReducer<SelectLeaderAction>
    {
        public GameState Reduce(GameState baseState, SelectLeaderAction action)
        {
            return baseState
                .Transform(action.Leader, AcquireLeader)
                .Transform(ResolveGameEndTrigger)
                .Transform(ResolveGameProgression);
        }
    }
}