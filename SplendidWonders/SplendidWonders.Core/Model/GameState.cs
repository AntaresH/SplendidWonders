using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace SplendidWonders.Core.Model
{
    public class GameState
    {
        public List<Player> Players { get; set; }

        public TokenGroup TokenSupply { get; set; }

        public ImmutableList<Leader> Leaders { get; set; }

        public CardGroup Level1 { get; set; }

        public CardGroup Level2 { get; set; }

        public CardGroup Level3 { get; set; }

        public Player CurrentPlayer { get; set; }

        public CardGroup this[int level]
        {
            get
            {
                return level switch
                {
                    1 => Level1,
                    2 => Level2,
                    3 => Level3,
                    _ => throw new ArgumentException("Level must be between 1 and 3")
                };
            }
        }
    }
}
