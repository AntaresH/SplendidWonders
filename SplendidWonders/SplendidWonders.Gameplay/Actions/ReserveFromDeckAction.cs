namespace SplendidWonders.Gameplay.Actions
{
    public class ReserveFromDeckAction : ReserveAction
    {
        public int Level { get; set; }

        public ReserveFromDeckAction() { Description = "Foglald le egy általad kiválasztott pakli legfelső kártyáját!"; }

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