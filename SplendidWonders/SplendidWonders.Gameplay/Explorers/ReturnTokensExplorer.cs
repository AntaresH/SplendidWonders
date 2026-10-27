using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Explorers
{
    public class ReturnTokensExplorer
    {
        public static IEnumerable<GameAction> GetActions(GameState gameState)
        {
            List<ReturnTokensAction> validReturnTokensActions = new List<ReturnTokensAction>();

            var playerStore = gameState.CurrentPlayer.Store;
            var tokenCount = playerStore.Yellow + playerStore.Blue + playerStore.Green + playerStore.Brown + playerStore.Red + playerStore.Polymath;

            if (tokenCount > 10)
            {
                List<ReturnTokensAction> returnTokensActions = new List<ReturnTokensAction>();

                var current = new int[5];

                var availableTokens = new[]
                {
                    playerStore.Yellow,
                    playerStore.Blue,
                    playerStore.Green,
                    playerStore.Brown,
                    playerStore.Red
                };

                GenerateVariations(availableTokens, current, 0, tokenCount - 10, returnTokensActions);
                validReturnTokensActions.AddRange(returnTokensActions);
            }

            return validReturnTokensActions;
        }

        private static void GenerateVariations(
                int[] available,
                int[] current,
                int colorIndex,
                int remaining,
                List<ReturnTokensAction> result)
        {
            {
                // All tokens have been allocated.
                if (colorIndex == 5)
                {
                    if (remaining == 0)
                    {
                        result.Add(
                            new ReturnTokensAction()
                            {
                                Tokens = TokenGroup.FromArray([current[0], current[1], current[2], current[3], current[4], 0])
                            });
                    }

                    return;
                }

                // Try every possible quantity of this color.
                int max = Math.Min(available[colorIndex], remaining);

                for (int count = 0; count <= max; count++)
                {
                    current[colorIndex] = count;
                    GenerateVariations(available, current, colorIndex + 1, remaining - count, result);
                }

                current[colorIndex] = 0;
            }
        }
    }
}
