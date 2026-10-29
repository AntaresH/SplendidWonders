using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests
{
    internal class GameStateBuilder
    {
        private Player _currentPlayer;

        private ImmutableList<Leader> _leaders;

        private CardGroup _level1;

        private CardGroup _level2;

        private CardGroup _level3;

        private List<Player> _players;

        private TokenGroup _tokenSupply;

        public GameStateBuilder WithCurrentPlayer(Player currentPlayer)
        {
            _currentPlayer = currentPlayer;
            return this;
        }

        public GameStateBuilder WithLeaders(ImmutableList<Leader> leaders)
        {
            _leaders = leaders;
            return this;
        }

        public GameStateBuilder WithLevel1(CardGroup level1)
        {
            _level1 = level1;
            return this;
        }

        public GameStateBuilder WithLevel2(CardGroup level2)
        {
            _level2 = level2;
            return this;
        }

        public GameStateBuilder WithLevel3(CardGroup level3)
        {
            _level3 = level3;
            return this;
        }

        public GameStateBuilder WithPlayers(List<Player> players)
        {
            _players = players;
            return this;
        }

        public GameStateBuilder WithTokenSupply(TokenGroup tokenSupply)
        {
            _tokenSupply = tokenSupply;
            return this;
        }

        public GameState Build()
        {
            return new GameState()
            {
                CurrentPlayer = _currentPlayer,
                Level1 = _level1,
                Level2 = _level2,
                Level3 = _level3,
                Leaders = _leaders,
                Players = _players,
                TokenSupply = _tokenSupply,
            };
        }

        public static GameStateBuilder Default
        {
            get
            {
                var defaultPlayers =
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    },
                    new Player()
                    {
                        Name = "Test player 2",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    },
                };

                var defaultLeaders = GameFactory.GetRandomizedLeaders(true, defaultPlayers.Count);
                var defaultCards = GameFactory.GetRandomizedCardSetup();
                var defaultTokenSupply = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]);

                return new GameStateBuilder()
                    .WithCurrentPlayer(defaultPlayers[0])
                    .WithLeaders(defaultLeaders)
                    .WithLevel1(defaultCards[0])
                    .WithLevel2(defaultCards[1])
                    .WithLevel3(defaultCards[2])
                    .WithPlayers(defaultPlayers)
                    .WithTokenSupply(defaultTokenSupply);
            }
        }
    }
}
