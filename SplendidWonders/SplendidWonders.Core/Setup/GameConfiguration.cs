namespace SplendidWonders.Core.Setup
{
    public class GameConfiguration
    {
        public List<string> PlayerNames { get; set; }
        public bool UseExtendedLeaders { get; set; }

        public bool UseRandomSeed { get; set; }

        public int Seed { get; set; }
    }
}