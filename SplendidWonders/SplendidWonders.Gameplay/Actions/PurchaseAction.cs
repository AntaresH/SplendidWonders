using SplendidWonders.Core.Model;

namespace SplendidWonders.Gameplay.Actions
{
    public abstract class PurchaseAction : CardAction
    {
    }

    public class PurchaseFromBoardAction : PurchaseAction
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

    public class PurchaseFromReservedAction : PurchaseAction
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