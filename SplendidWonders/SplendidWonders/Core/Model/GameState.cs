using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Model
{
    internal class GameState
    {
        public IReadOnlyList<Player> Players { get; set; }

        public TokenGroup TokenSupply { get; set; }

        public IReadOnlyList<Leader> AvailableLeaders { get; set; }

        public IReadOnlyList<DevelopmentCard> Level1Deck { get; set; }
        public IReadOnlyList<DevelopmentCard> Level2Deck { get; set; }
        public IReadOnlyList<DevelopmentCard> Level3Deck { get; set; }

        public IReadOnlyList<DevelopmentCard> Level1AvailableCards { get; set; }
        public IReadOnlyList<DevelopmentCard> Level2AvailableCards { get; set; }
        public IReadOnlyList<DevelopmentCard> Level3AvailableCards { get; set; }
    }
}
