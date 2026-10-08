using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpendidWonders.Core.Setup
{
    public class GameFactory
    {
        public static GameState CreateGame(GameConfiguration configuration, int numberOfPlayers)
        {
            var players = new List<Player>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(new Player());
            }

            var tokenSupply = numberOfPlayers switch
            {
                2 => new TokenGroup() { Yellow = 4, Blue = 4, Green = 4, Brown = 4, Red = 4, Polymath = 5 },
                3 => new TokenGroup() { Yellow = 5, Blue = 5, Green = 5, Brown = 5, Red = 5, Polymath = 5 },
                4 => new TokenGroup() { Yellow = 7, Blue = 7, Green = 7, Brown = 7, Red = 7, Polymath = 5 },
                _ => throw new ArgumentException("Invalid number of players. Must be 2, 3, or 4.")
            };

            var availableLeaders = configuration.UseExtendedLeaders
                ? Leaders.StandardLeaders.Concat(Leaders.ExtendedLeaders).ToList()
                : Leaders.StandardLeaders.ToList();

            var leaderDeck = configuration.UseExtendedLeaders
                ? Leaders.StandardLeaders.Concat(Leaders.ExtendedLeaders).ToList()
                : Leaders.StandardLeaders.ToList();

            var decks = new CardGroup
            {
                Level1 = Cards.Level1.Shuffle().ToList(),
                Level2 = Cards.Level2.Shuffle().ToList(),
                Level3 = Cards.Level3.Shuffle().ToList()
            };

            var market = new CardGroup
            {
                Level1 = decks.Level1.Take(4).ToList(),
                Level2 = decks.Level2.Take(4).ToList(),
                Level3 = decks.Level3.Take(4).ToList()
            };

            return new GameState
            {
                Players = players,
                TokenSupply = tokenSupply,
                Leaders = availableLeaders,
                Decks = decks,
                Market = market
            };
        }
    }
}
