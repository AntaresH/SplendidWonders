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
        public static GameState GetNewGameState(GameConfiguration configuration)
        {
            var players = new List<Player>();

            for (int i = 0; i < configuration.PlayerNames.Count; i++)
            {
                players.Add(new Player() { Id = i, Name = configuration.PlayerNames[i] });
            }

            var tokenSupply = configuration.PlayerNames.Count switch
            {
                2 => new TokenGroup() { Yellow = 4, Blue = 4, Green = 4, Brown = 4, Red = 4, Polymath = 5 },
                3 => new TokenGroup() { Yellow = 5, Blue = 5, Green = 5, Brown = 5, Red = 5, Polymath = 5 },
                4 => new TokenGroup() { Yellow = 7, Blue = 7, Green = 7, Brown = 7, Red = 7, Polymath = 5 },
                _ => throw new ArgumentException("Invalid number of players. Must be 2, 3, or 4.")
            };

            int seed = configuration.UseRandomSeed ? new Random().Next(1,1000000) : configuration.Seed;

            var randomizedDecks = GetRandomizedCardSetup(seed);
            var randomizedLeaders = GetRandomizedLeaders(configuration.UseExtendedLeaders, configuration.PlayerNames.Count, seed);

            return new GameState
            {
                Players = players,
                TokenSupply = tokenSupply,
                Leaders = randomizedLeaders,
                Level1 = randomizedDecks[0],
                Level2 = randomizedDecks[1],
                Level3 = randomizedDecks[2],
                CurrentPlayer = players[0]
            };
        }

        public static ImmutableList<Leader> GetRandomizedLeaders(bool useExtendedLeaders, int playerCount, int seed = 0)
        {
            IEnumerable<Leader> randomizedLeaders = useExtendedLeaders
                ? Leaders.StandardLeaders.Concat(Leaders.ExtendedLeaders)
                : Leaders.StandardLeaders;

            if (seed != 0)
                randomizedLeaders = randomizedLeaders.Shuffle(seed);

            return randomizedLeaders.Take(playerCount + 1).ToImmutableList();
        }

        public static CardGroup[] GetRandomizedCardSetup(int seed = 0)
        {
            CardGroup[] randomSetup = new CardGroup[3];

            IEnumerable<DevelopmentCard> level1Cards = Cards.Level1;
            
            if (seed != 0)
             level1Cards = level1Cards.Shuffle(seed + 1);

            randomSetup[0] = new CardGroup()
            {
                Deck = level1Cards.Skip(4).ToImmutableList(),
                Market = level1Cards.Take(4).ToImmutableList()
            };

            IEnumerable<DevelopmentCard> level2Cards = Cards.Level2;

            if (seed != 0)
                level2Cards = level2Cards.Shuffle(seed + 2);

            randomSetup[1] = new CardGroup()
            {
                Deck = level2Cards.Skip(4).ToImmutableList(),
                Market = level2Cards.Take(4).ToImmutableList()
            };

            IEnumerable<DevelopmentCard> level3Cards = Cards.Level3;

            if (seed != 0)
                level3Cards = level3Cards.Shuffle(seed + 3);

            randomSetup[2] = new CardGroup()
            {
                Deck = level3Cards.Skip(4).ToImmutableList(),
                Market = level3Cards.Take(4).ToImmutableList()
            };

            return randomSetup;
        }
    }
}
