using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Security;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public class GameFactory
    {
        public static GameState GetNewGameState(GameConfiguration configuration, List<string> playerNames)
        {
            var players = new List<Player>();

            for (int i = 0; i < playerNames.Count; i++)
            {
                players.Add(new Player() { Name = playerNames[i] });
            }

            var tokenSupply = playerNames.Count switch
            {
                2 => new TokenGroup() { Yellow = 4, Blue = 4, Green = 4, Brown = 4, Red = 4, Polymath = 5 },
                3 => new TokenGroup() { Yellow = 5, Blue = 5, Green = 5, Brown = 5, Red = 5, Polymath = 5 },
                4 => new TokenGroup() { Yellow = 7, Blue = 7, Green = 7, Brown = 7, Red = 7, Polymath = 5 },
                _ => throw new ArgumentException("Invalid number of players. Must be 2, 3, or 4.")
            };

            var availableLeaders = configuration.UseExtendedLeaders
                ? Leaders.StandardLeaders.Concat(Leaders.ExtendedLeaders).ToImmutableList()
                : Leaders.StandardLeaders.ToImmutableList();

            var leaderDeck = configuration.UseExtendedLeaders
                ? Leaders.StandardLeaders.Concat(Leaders.ExtendedLeaders).ToList()
                : Leaders.StandardLeaders.ToList();

            var level1Cards = Cards.Level1.Shuffle();

            var level1 = new CardGroup()
            {
                Deck = level1Cards.Skip(4).ToImmutableList(),
                Market = level1Cards.Take(4).ToImmutableList()
            };

            var level2Cards = Cards.Level1.Shuffle();

            var level2 = new CardGroup()
            {
                Deck = level2Cards.Skip(4).ToImmutableList(),
                Market = level2Cards.Take(4).ToImmutableList()
            };
            var level3Cards = Cards.Level1.Shuffle();

            var level3 = new CardGroup()
            {
                Deck = level3Cards.Skip(4).ToImmutableList(),
                Market = level3Cards.Take(4).ToImmutableList()
            };

            return new GameState
            {
                Players = players,
                TokenSupply = tokenSupply,
                Leaders = availableLeaders,
                Level1 = level1,
                Level2 = level2,
                Level3 = level3,
                CurrentPlayer = players[0]
            };
        }
    }
}
