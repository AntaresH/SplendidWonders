using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace SplendidWonders.Gameplay.Reducers
{
    public class DrawThreeTokensReducer : IStateReducer<DrawThreeTokensAction>
    {
        public GameState Reduce(GameState baseState, DrawThreeTokensAction action)
        {
            var playerStore = baseState.CurrentPlayer.Store.ToArray();
            var selectedTokens = action.Tokens.ToArray();
            var tokenSupply = baseState.TokenSupply.ToArray();

            var reducedStateBuilder = new GameStateBuilder(baseState);

            for (int i = 0; i < 5; i++)
            {
                playerStore[i] += selectedTokens[i];
                tokenSupply[i] -= selectedTokens[i];
            }

            reducedStateBuilder.WithPlayerStore(baseState.CurrentPlayer.Id, TokenGroup.FromArray(playerStore));
            reducedStateBuilder.WithTokenSupply(TokenGroup.FromArray(tokenSupply));

            int allPlayerTokens = playerStore[0] + playerStore[1] + playerStore[2] + playerStore[3] + playerStore[4] + playerStore[5];
            var currentPlayerId = allPlayerTokens <= 10 
                ? baseState.CurrentPlayer.Id + 1 % baseState.Players.Count 
                : baseState.CurrentPlayer.Id;

            reducedStateBuilder.WithCurrentPlayer(currentPlayerId);
            return reducedStateBuilder.Build();
        }
    }
}
