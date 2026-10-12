using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SplendidWonders.Core.Model
{
    /// <summary>
    /// Simple deck abstraction that encapsulates an ordered collection of items
    /// and exposes draw/shuffle operations. The first item in the provided
    /// sequence is treated as the top of the deck.
    /// </summary>
    public class Deck<T> : IReadOnlyList<T>
    {
        private readonly List<T> _cards;

        public Deck()
        {
            _cards = new List<T>();
        }

        public Deck(IEnumerable<T> initial, bool topIsFirst = true)
        {
            if (initial == null) throw new ArgumentNullException(nameof(initial));
            _cards = topIsFirst ? new List<T>(initial) : new List<T>(initial.Reverse());
        }

        public int Count => _cards.Count;

        /// <summary>
        /// Indexer to provide read-only indexed access (top at index 0).
        /// This makes Deck&lt;T&gt; compatible with code that used IReadOnlyList&lt;T&gt; indexing.
        /// </summary>
        public T this[int index] => _cards[index];

        /// <summary>
        /// Draw up to <paramref name="count"/> cards from the top of the deck.
        /// If fewer cards are available, all available cards are returned.
        /// </summary>
        public List<T> Draw(int count)
        {
            if (count <= 0) return new List<T>();
            var actual = Math.Min(count, _cards.Count);
            var result = _cards.GetRange(0, actual);
            _cards.RemoveRange(0, actual);
            return result;
        }

        /// <summary>
        /// Peek at the top card without removing it. Returns true if a card
        /// was available.</summary>
        public bool TryPeek(out T card)
        {
            if (_cards.Count > 0)
            {
                card = _cards[0];
                return true;
            }
            card = default!;
            return false;
        }

        /// <summary>
        /// Put cards onto the bottom of the deck (they will be drawn last).
        /// </summary>
        public void AddToBottom(IEnumerable<T> cards)
        {
            if (cards == null) return;
            _cards.AddRange(cards);
        }

        /// <summary>
        /// Put cards onto the top of the deck in the order provided (first becomes top-most).
        /// </summary>
        public void AddToTop(IEnumerable<T> cards)
        {
            if (cards == null) return;
            var list = cards as ICollection<T> ?? cards.ToList();
            _cards.InsertRange(0, list);
        }

        /// <summary>
        /// Shuffle the deck in-place using Fisher-Yates.
        /// </summary>
        public void Shuffle(Random? rng = null)
        {
            rng ??= new Random();
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = _cards[i];
                _cards[i] = _cards[j];
                _cards[j] = tmp;
            }
        }

        /// <summary>
        /// Returns a read-only snapshot of the remaining cards (top at index 0).
        /// </summary>
        public IReadOnlyList<T> AsReadOnly() => _cards.AsReadOnly();

        /// <summary>
        /// Empties the deck and returns all remaining cards (top first).
        /// </summary>
        public List<T> DrawAll()
        {
            var res = new List<T>(_cards);
            _cards.Clear();
            return res;
        }

        public IEnumerator<T> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _cards.GetEnumerator();
    }
}
