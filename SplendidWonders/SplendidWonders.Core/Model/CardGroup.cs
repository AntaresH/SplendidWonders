using System;

namespace SplendidWonders.Core.Model
{
    public class CardGroup
    {
        public Deck<DevelopmentCard> Level1 { get; set; }
        public Deck<DevelopmentCard> Level2 { get; set; }
        public Deck<DevelopmentCard> Level3 { get; set; }

        // Enables usage like: gameState.Market[level][column] where level is 1..3 (as in GameExecutor.cs)
        public Deck<DevelopmentCard> this[int level]
        {
            get => level switch
            {
                1 => Level1,
                2 => Level2,
                3 => Level3,
                _ => throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 3.")
            };
        }

        // Two-dimensional accessor: use 1-based level and column to match Console usage (e.g. Market[level][column])
        public DevelopmentCard this[int level, int column]
        {
            get
            {
                var list = this[level]; // will validate level
                if (column < 1 || column > list.Count)
                    throw new ArgumentOutOfRangeException(nameof(column), $"Column must be between 1 and {list.Count}.");
                return list[column - 1]; // convert to 0-based index
            }
        }
    }
}