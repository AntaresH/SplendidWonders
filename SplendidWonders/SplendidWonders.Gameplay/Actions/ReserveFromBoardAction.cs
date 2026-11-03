using SplendidWonders.Core.Model;

namespace SplendidWonders.Gameplay.Actions
{
    public class ReserveFromBoardAction : CardAction
    {
        public override bool Equals(object? obj)
        {
            if (obj is ReserveFromBoardAction other)
            {
                return Card.Equals(other.Card);
            }

            return base.Equals(obj);
        }
    }
}