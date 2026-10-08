namespace SpendidWonders.Core.Setup
{
    public class GameConfiguration
    {
        public static GameConfiguration Standard => new GameConfiguration() { UseExtendedLeaders = false };
        public static GameConfiguration ExtendedLeaders => new GameConfiguration() { UseExtendedLeaders = true };

        public bool UseExtendedLeaders { get; internal set; }
    }
}