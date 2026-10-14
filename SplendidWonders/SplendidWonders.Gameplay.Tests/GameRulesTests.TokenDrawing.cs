using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Tests
{
    public partial class GameRulesTests
    {
        private static IEnumerable<TestCaseData> DrawThreeTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1]) },
                })
                .SetName("Draw Three Tokens - All are available");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 4, 4]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1]) },
                })
                .SetName("Draw Three Tokens - Yellow unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 4, 0, 0, 4]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1]) },
                })
                .SetName("Draw Three Tokens - Green and red unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 0, 0, 0, 4]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1]) },
                })                
                .SetName("Draw Three Tokens - Blue and green and brown unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Only brown is available");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 0, 0]), new DrawThreeTokensAction[]{ })
                .SetName("Draw Three Tokens - Nothing is available");
        }
    }
}
