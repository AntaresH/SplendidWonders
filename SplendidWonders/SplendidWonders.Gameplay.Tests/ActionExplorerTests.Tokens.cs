using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests
{
    public partial class ActionExplorerTests
    {
        private static IEnumerable<TestCaseData> DrawThreeTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 1, 0, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1, 0]) },
                })
                .SetName("Draw Three Tokens - All are available");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 4, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 4, 0, 4, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Green unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 4, 0, 0, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Green and brown unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 4, 0, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow and red unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 0, 0, 0, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Blue and green and brown unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 0, 0, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 0, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow and brown and red unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 4, 0, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0, 0]) },
                })
                .SetName("Draw Three Tokens - Only brown is available");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new DrawThreeTokensAction[] { })
                .SetName("Draw Three Tokens - Nothing is available");
        }

        private static IEnumerable<TestCaseData> DrawDoubleTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) }
                })
                .SetName("Draw Double Tokens - All token types are available (>4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 4, 4, 4, 0]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) }
                })
                .SetName("Draw Double Tokens - Blue are unavailable (<4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 4, 2, 1, 0]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) }
                })
                .SetName("Draw Double Tokens - Blue, Brown and Red are unavailable (<4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 3, 0, 2, 1, 0]),
                new DrawDoubleTokensAction[] { })
                .SetName("Draw Double Tokens - No double draw is available (<4)");
        }

        private static IEnumerable<TestCaseData> ReturnTokensActionCases()
        {
            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([2, 2, 2, 2, 3, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 1, 0]) }
                })
                .SetName("Return Tokens - 11 player tokens, balanced");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([4, 2, 5, 0, 0, 0]),
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0, 0]) }
                })
                .SetName("Return Tokens - 11 player tokens, no brown and red tokens");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([3, 3, 2, 2, 2, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 1, 0]) },
                })
                .SetName("Return Tokens - 12 player tokens, balanced");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([4, 0, 3, 2, 3, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 1, 0]) },
                })
                .SetName("Return Tokens - 12 player tokens, no blue tokens");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([3, 2, 3, 3, 2, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([3, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 3, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 3, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 2, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1, 0]) },
                })
                .SetName("Return Tokens - 13 player tokens, balanced");
        }
    }
}
