using SplendidWonders.Core.Model;
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
    }
}
