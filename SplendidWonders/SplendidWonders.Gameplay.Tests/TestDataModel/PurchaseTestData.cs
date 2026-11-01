using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests.TestDataModel
{
    public class PurchaseTestData
    {
        public TokenGroup TokenSupply { get; set; }
        public TokenGroup PlayerStore { get; set; }
        public required ImmutableList<DevelopmentCard> PlayerReservedCards { get; set; }
        public required ImmutableList<DevelopmentCard> PlayerTableau { get; set; }
        public int ActivePlayerId { get; set; }
    }
}
