using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay
{
    public class GameStateBuilder
    {
        internal Player _currentPlayer;

        internal ImmutableList<Leader> _leaders;

        internal CardGroup _level1;

        internal CardGroup _level2;

        internal CardGroup _level3;

        internal List<Player> _players;

        internal TokenGroup _tokenSupply;

        public GameStateBuilder() { }

        public GameStateBuilder(GameState existingState)
        {
            _currentPlayer = existingState.CurrentPlayer;
            _leaders = existingState.Leaders;
            _level1 = existingState.Level1;
            _level2 = existingState.Level2;
            _level3 = existingState.Level3;
            _players = existingState.Players;
            _tokenSupply = existingState.TokenSupply;
        }

        public GameStateBuilder WithCurrentPlayer(int currentPlayerId)
        {
            _currentPlayer = _players.First(p => p.Id == currentPlayerId);
            return this;
        }

        public GameStateBuilder WithLeaders(ImmutableList<Leader> leaders)
        {
            _leaders = leaders;
            return this;
        }

        public GameStateBuilder WithLevel(int level, CardGroup cardGroup)
        {
            return level switch
            {
                1 => WithLevel1(cardGroup),
                2 => WithLevel2(cardGroup),
                3 => WithLevel3(cardGroup),
                _ => throw new ArgumentException("Level must between 1 and 3!")
            };
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

        public GameStateBuilder WithPlayerStore(int playerId, TokenGroup store)
        {
            var player = _players.First(p => p.Id == playerId);
            player.Store = store;
            return this;
        }

        public GameStateBuilder WithPlayerReservedCards(int playerId, ImmutableList<DevelopmentCard> reservedCards)
        {
            var player = _players.First(p => p.Id == playerId);
            player.ReservedCards = reservedCards;
            return this;
        }

        public GameStateBuilder WithPlayerTableau(int playerId, ImmutableList<DevelopmentCard> tableau)
        {
            var player = _players.First(p => p.Id == playerId);
            player.Tableau = tableau;
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
                        Id = 1,
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    },
                    new Player()
                    {
                        Id = 2,
                        Name = "Test player 2",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    },
                };

                var defaultLeaders = GameFactory.GetRandomizedLeaders(true, defaultPlayers.Count);
                var defaultCards = GameFactory.GetRandomizedCardSetup();
                var defaultTokenSupply = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]);

                return new GameStateBuilder()
                    .WithLeaders(defaultLeaders)
                    .WithLevel1(defaultCards[0])
                    .WithLevel2(defaultCards[1])
                    .WithLevel3(defaultCards[2])
                    .WithPlayers(defaultPlayers)
                    .WithCurrentPlayer(1)
                    .WithTokenSupply(defaultTokenSupply);
            }
        }
    }
}
