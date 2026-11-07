using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace SplendidWonders.Core.Model
{
    public record class GameState
    {
        public required List<Player> Players { get; set; }

        public TokenGroup TokenSupply { get; set; }

        public required ImmutableList<Leader> Leaders { get; set; }

        public required CardGroup Level1 { get; set; }

        public required CardGroup Level2 { get; set; }

        public required CardGroup Level3 { get; set; }

        int _activePlayerId = 1;

        public required Player ActivePlayer
        {
            get
            {
                return Players[_activePlayerId - 1];
            }
            set
            {
                _activePlayerId = value.Id;
            }
        }

        public GameProgression Progression { get; set; }

        public bool IsFinalTurn { get; set; }

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
