namespace SplendidWonders.Core.Model
{
    public class Player
    {
        public string Name { get; internal set; }

        public TokenGroup Store { get; set; } = new();

        public IReadOnlyList<DevelopmentCard> Tableau { get; set; } = Array.Empty<DevelopmentCard>();

        public IReadOnlyList<DevelopmentCard> ReservedCards { get; set; } = Array.Empty<DevelopmentCard>();

        public IReadOnlyList<Leader> AcquiredLeaders { get; set; } = Array.Empty<Leader>();
    }
}