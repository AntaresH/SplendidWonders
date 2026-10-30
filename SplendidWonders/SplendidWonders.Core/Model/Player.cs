using System.Collections.Immutable;

namespace SplendidWonders.Core.Model
{
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TokenGroup Store { get; set; } = new();

        public ImmutableList<DevelopmentCard> Tableau { get; set; } = ImmutableList<DevelopmentCard>.Empty;

        public ImmutableList<DevelopmentCard> ReservedCards { get; set; } = ImmutableList<DevelopmentCard>.Empty;

        public ImmutableList<Leader> AcquiredLeaders { get; set; } = ImmutableList<Leader>.Empty;
    }
}