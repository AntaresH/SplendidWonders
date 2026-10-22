using SplendidWonders.Core.Model;

namespace SplendidWonders.Gameplay.Actions
{
    public class PurchaseAction : GameAction
    {
        public DevelopmentCard Card { get; set; }

        public PurchaseAction() { Description = "Vásárolj meg egy kártyát a játéktérről vagy a korábban általad foglaltak közül!"; }

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