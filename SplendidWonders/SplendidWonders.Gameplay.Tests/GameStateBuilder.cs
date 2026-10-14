using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Tests
{
    internal class GameStateBuilder
    {
        private TokenGroup _tokenSupply;

        public GameStateBuilder()
        {

        }

        public GameStateBuilder WithLeaders()
        {
            return this;
        }

        public GameStateBuilder WithMarket()
        {
            return this;
        }

        public GameStateBuilder WithDecks()
        {
            return this;
        }

        public GameStateBuilder WithPlayers()
        {
            return this;
        }

        public GameStateBuilder WithCurrentPlayer()
        {
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
                TokenSupply = _tokenSupply
            };
        }
    }
}
