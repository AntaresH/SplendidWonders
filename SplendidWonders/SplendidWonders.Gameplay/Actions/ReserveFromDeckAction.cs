namespace SplendidWonders.Gameplay.Actions
{
    public class ReserveFromDeckAction : GameAction
    {
        public int Level { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is ReserveFromDeckAction other)
            {
                return Level == other.Level;
            }

            return base.Equals(obj);
        }
    }
}