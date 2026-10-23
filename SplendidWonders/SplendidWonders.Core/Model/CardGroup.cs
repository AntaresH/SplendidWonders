using System;
using System.Collections.Immutable;

namespace SplendidWonders.Core.Model
{
    public class CardGroup
    {
        public required ImmutableList<DevelopmentCard> Deck { get; set; }

        public required ImmutableList<DevelopmentCard> Market { get; set; }

        public void Draw(int marketIndex)
        {
            if (Deck.IsEmpty)
                return;

            var card = Deck[0];
            
            Deck.RemoveAt(0);
            Market.Insert(marketIndex, card);
        }

        public DevelopmentCard? this[int index]
        {
            get
            {
                if (index < 1 || index > 4)
                    throw new ArgumentException("Column must be between 1 and 4");

                return Market[index - 1];
            }
        }

        public int Remaining
        {
            get
            {
                return Deck.Count;
            }
        }
    }
}