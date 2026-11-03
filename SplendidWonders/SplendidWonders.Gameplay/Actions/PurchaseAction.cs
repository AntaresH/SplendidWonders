using SplendidWonders.Core.Model;

namespace SplendidWonders.Gameplay.Actions
{
    public class PurchaseAction : CardAction
    {
        public override bool Equals(object? obj)
        {
            if (obj is PurchaseAction other)
            {
                return Card.Equals(other.Card);
            }

            return base.Equals(obj);
        }
    }
}