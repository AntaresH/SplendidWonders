using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
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

            yield return new TestCaseData(TokenGroup.FromArray([4, 4, 0, 4, 4]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1]) },
                })
                .SetName("Draw Three Tokens - Green unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 4, 0, 0, 4]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1]) },
                })
                .SetName("Draw Three Tokens - Green and brown unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow and red unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 0, 0, 0, 4]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1]) },
                })
                .SetName("Draw Three Tokens - Blue and green and brown unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 0, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow and brown and red unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Only brown is available");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 0, 0]), new DrawThreeTokensAction[] { })
                .SetName("Draw Three Tokens - Nothing is available");
        }

        private static IEnumerable<TestCaseData> DrawDoubleTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2]) }
                })
                .SetName("Draw Double Tokens - All token types are available (>4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 4, 4, 4]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2]) }
                })
                .SetName("Draw Double Tokens - Blue are unavailable (<4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 4, 2, 1]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0]) }
                })
                .SetName("Draw Double Tokens - Blue, Brown and Red are unavailable (<4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 3, 0, 2, 1]), new DrawDoubleTokensAction[] { })
                .SetName("Draw Double Tokens - No double draw is available (<4)");
        }

        private static IEnumerable<TestCaseData> ReturnTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([2, 2, 2, 2, 3]),
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 1]) }
                })
                .SetName("Return Tokens - 11 player tokens, balanced");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 2, 5, 0, 0]),
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0]) }
                })
                .SetName("Return Tokens - 11 player tokens, no brown and red tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([3, 3, 2, 2, 2]),
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 1]) },
                })
                .SetName("Return Tokens - 12 player tokens, balanced");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 0, 3, 2, 3]),
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 1]) },
                })
                .SetName("Return Tokens - 12 player tokens, no blue tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 2, 3, 1]),
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([3, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 3, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 3, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 1, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 2]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 2]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 2]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 2]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1]) },
                })
                .SetName("Return Tokens - 13 player tokens, balanced");
        }

        private static IEnumerable<TestCaseData> ReserveFromBoardActionCases()
        {
            var decks = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(Cards.Level1.Shuffle().ToList()),
                Level2 = new Deck<DevelopmentCard>(Cards.Level2.Shuffle().ToList()),
                Level3 = new Deck<DevelopmentCard>(Cards.Level3.Shuffle().ToList())
            };

            var fullMarket = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(decks.Level1.Draw(4)),
                Level2 = new Deck<DevelopmentCard>(decks.Level2.Draw(4)),
                Level3 = new Deck<DevelopmentCard>(decks.Level3.Draw(4))
            };

            var shortMarket1 = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(decks.Level1.Draw(3)),
                Level2 = new Deck<DevelopmentCard>(decks.Level2.Draw(4)),
                Level3 = new Deck<DevelopmentCard>(decks.Level3.Draw(4))
            };

            var shortMarket2 = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(decks.Level1.Draw(4)),
                Level2 = new Deck<DevelopmentCard>(decks.Level2.Draw(3)),
                Level3 = new Deck<DevelopmentCard>(decks.Level3.Draw(3))
            };

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = new List<DevelopmentCard>(),
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                fullMarket,
                new[]
                {
                    new ReserveFromBoardAction() { Card = fullMarket[1, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 4] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 4] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + full market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = decks.Level1.Draw(3),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0])
                    }
                },
                fullMarket,
                new ReserveFromBoardAction[] {})
                .SetName("Reserve From Board - Three  reserved cards + full market");


            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = new List<DevelopmentCard>(),
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                shortMarket1,
                new[]
                {
                    new ReserveFromBoardAction() { Card = fullMarket[1, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 4] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + short market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = new List<DevelopmentCard>(),
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                shortMarket2,
                new[]
                {
                    new ReserveFromBoardAction() { Card = fullMarket[1, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[1, 4] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[2, 3] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 1] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 2] },
                    new ReserveFromBoardAction() { Card = fullMarket[3, 3] },
                })
                .SetName("Reserve From Board - Zero reserved cards + very short market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = decks.Level1.Draw(3),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0])
                    }
                },
                shortMarket1,
                new ReserveFromBoardAction[] { })
                .SetName("Reserve From Board - Three  reserved cards + short market");
        }
    }
}
