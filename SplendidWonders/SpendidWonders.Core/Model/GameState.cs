using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Model
{
    public class GameState
    {
        public List<Player> Players { get; set; }

        public TokenGroup TokenSupply { get; set; }

        public List<Leader> Leaders { get; set; }

        public CardGroup Decks { get; set; }

        public Market Market { get; set; }
    }
}
