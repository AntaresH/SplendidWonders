using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Explorers
{
    public class DrawThreeTokensExplorer
    {
        public static IEnumerable<GameAction> GetActions(GameState gameState)
        {
            List<DrawThreeTokensAction> validDrawThreeActions = new List<DrawThreeTokensAction>();

            bool[] emptyColors =
            {
                gameState.TokenSupply.Yellow == 0,
                gameState.TokenSupply.Blue == 0,
                gameState.TokenSupply.Green == 0,
                gameState.TokenSupply.Brown == 0,
                gameState.TokenSupply.Red == 0,
            };

            if (emptyColors.Count(true) < 2)
            {
                var permutationActions = CalculateThreeTokensActionPermutations(emptyColors);
                validDrawThreeActions.AddRange(permutationActions);
            }
            else if (emptyColors.Count(true) < 5)
            {
                var values = new int[6];

                for (int i = 0; i < emptyColors.Length; i++)
                {
                    values[i] = emptyColors[i] ? 0 : 1;
                }

                validDrawThreeActions.Add(new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray(values) });
            }

            return validDrawThreeActions;
        }

        private static IEnumerable<DrawThreeTokensAction> CalculateThreeTokensActionPermutations(bool[] emptyColors)
        {
            List<DrawThreeTokensAction> permutationActions = new List<DrawThreeTokensAction>();

            for (int i = 0; i < 5; i++)
            {
                if (emptyColors[i])
                    continue;

                for (int j = i + 1; j < 5; j++)
                {
                    if (emptyColors[j])
                        continue;

                    for (int k = j + 1; k < 5; k++)
                    {
                        if (emptyColors[k])
                            continue;

                        var values = new int[6];
                        values[i] = 1;
                        values[j] = 1;
                        values[k] = 1;

                        var group = TokenGroup.FromArray(values);
                        permutationActions.Add(new DrawThreeTokensAction() { Tokens = group });
                    }
                }
            }

            return permutationActions;
        }
    }
}
