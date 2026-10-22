using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Tests
{
    internal class GameStateBuilder
    {
        private TokenGroup _tokenSupply;

        private CardGroup _market;

        private Player _currentPlayer;

        private List<Player> _players;
        private CardGroup _decks;

        public GameStateBuilder()
        {

        }

        public GameStateBuilder WithLeaders()
        {
            return this;
        }

        public GameStateBuilder WithMarket(CardGroup market)
        {
            _market = market;
            return this;
        }

        public GameStateBuilder WithDecks(CardGroup decks)
        {
            _decks = decks;
            return this;
        }

        public GameStateBuilder WithPlayers(List<Player> players)
        {
            _players = players;
            return this;
        }

        public GameStateBuilder WithCurrentPlayer(Player currentPlayer)
        {
            _currentPlayer = currentPlayer;
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
                Decks = _decks,
                Players = _players,
                Market =_market,
                TokenSupply = _tokenSupply,
            };
        }
    }
}
