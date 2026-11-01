using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests.TestDataModel
{
    public record class ReducerTestData
    {
        public TokenGroup TokenSupply { get; set; }
        public ImmutableList<Leader> AvailableLeaders { get; set; } = ImmutableList<Leader>.Empty;

        public TokenGroup PlayerStore { get; set; }
        public ImmutableList<DevelopmentCard> PlayerReservedCards { get; set; } = ImmutableList<DevelopmentCard>.Empty;
        public ImmutableList<DevelopmentCard> PlayerTableau { get; set; } = ImmutableList<DevelopmentCard>.Empty;
        public ImmutableList<Leader> PlayerAcquiredLeaders { get; set; } = ImmutableList<Leader>.Empty;

        public int ActivePlayerId { get; set; }
    }
}
