using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay
{
    public static class GameStateExtensions
    {
        public static GameState Transform<TAction>(this GameState state, TAction action, Func<GameState, TAction, GameState> transform)
        {
            ArgumentNullException.ThrowIfNull(transform);

            return transform(state, action);
        }
        public static GameState Transform(this GameState state, Func<GameState, GameState> transform)
        {
            ArgumentNullException.ThrowIfNull(transform);

            return transform(state);
        }

        public static GameState UpdatePlayerData(this GameState state, int playerIndex, TokenGroup store, ImmutableList<DevelopmentCard> reservedCards, ImmutableList<DevelopmentCard> tableau, ImmutableList<Leader> acquiredLeaders)
        {
            var updatedPlayers = new List<Player>(state.Players);
            updatedPlayers[playerIndex - 1] = updatedPlayers[playerIndex - 1] with { Store = store, ReservedCards = reservedCards, Tableau = tableau, AcquiredLeaders = acquiredLeaders };

            return state with { Players = updatedPlayers };
        }

        public static GameState UpdateTokenSupply(this GameState state, TokenGroup tokenSupply)
        {
            return state with { TokenSupply = tokenSupply };
        }

        public static GameState UpdateLevel(this GameState state, int level, CardGroup cardGroup)
        {
            return level switch
            {
                1 => state with { Level1 = cardGroup },
                2 => state with { Level2 = cardGroup },
                3 => state with { Level3 = cardGroup },
                _ => throw new ArgumentException("Level must be between 1 and 3")
            };
        }

        public static GameState UpdateAvailableLeaders(this GameState state, ImmutableList<Leader> availableLeaders)
        {
            return state with { Leaders = availableLeaders };
        }
    }
}
