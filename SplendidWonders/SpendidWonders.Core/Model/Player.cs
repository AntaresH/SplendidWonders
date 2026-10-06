namespace SplendidWonders.Core.Model
{
    public class Player
    {
        public TokenGroup Store { get; set; }

        public IReadOnlyList<DevelopmentCard> Tableau { get; set; }

        public IReadOnlyList<DevelopmentCard> ReservedCards { get; set; }

        public IReadOnlyList<Leader> AcquiredLeaders { get; set; }
    }
}