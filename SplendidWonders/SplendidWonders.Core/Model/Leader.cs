namespace SplendidWonders.Core.Model
{
    public class Leader
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int VictoryPoints { get; set; }

        public TokenGroup Requirements { get; set; }
    }
}