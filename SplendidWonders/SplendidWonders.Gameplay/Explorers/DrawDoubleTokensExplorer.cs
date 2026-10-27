using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Explorers
{
    public class DrawDoubleTokensExplorer
    {
        public static IEnumerable<GameAction> GetActions(GameState gameState)
        {
            List<DrawDoubleTokensAction> gameActions = new List<DrawDoubleTokensAction>();

            if (gameState.TokenSupply.Yellow >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 2, 0, 0, 0, 0, 0 }) });
            if (gameState.TokenSupply.Blue >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 2, 0, 0, 0, 0 }) });
            if (gameState.TokenSupply.Green >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 2, 0, 0, 0 }) });
            if (gameState.TokenSupply.Brown >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 2, 0, 0 }) });
            if (gameState.TokenSupply.Red >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 0, 2, 0 }) });

            return gameActions;
        }
    }
}
