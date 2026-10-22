using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Tests
{
    public partial class ActionExplorerTests
    {

        private static IEnumerable<TestCaseData> ReserveFromBoardActionCases()
        {
            var decks = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(Cards.Level1.ToList()),
                Level2 = new Deck<DevelopmentCard>(Cards.Level2.ToList()),
                Level3 = new Deck<DevelopmentCard>(Cards.Level3.ToList())
            };

            var fullMarket = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(decks.Level1.Draw(4)),
                Level2 = new Deck<DevelopmentCard>(decks.Level2.Draw(4)),
                Level3 = new Deck<DevelopmentCard>(decks.Level3.Draw(4))
            };

            var shortMarket1 = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(fullMarket.Level1.Take(3)),
                Level2 = new Deck<DevelopmentCard>(fullMarket.Level2),
                Level3 = new Deck<DevelopmentCard>(fullMarket.Level3)
            };

            var shortMarket2 = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(fullMarket.Level1),
                Level2 = new Deck<DevelopmentCard>(fullMarket.Level2.Take(3)),
                Level3 = new Deck<DevelopmentCard>(fullMarket.Level3.Take(3))
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

        private static IEnumerable<TestCaseData> ReserveFromDeckActionCases()
        {
            var decks = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(Cards.Level1.Skip(10).ToList()),
                Level2 = new Deck<DevelopmentCard>(Cards.Level2.Skip(10).ToList()),
                Level3 = new Deck<DevelopmentCard>(Cards.Level3.Skip(10).ToList())
            };

            var decksOneEmpty = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(),
                Level2 = decks.Level2,
                Level3 = decks.Level3,
            };

            var decksMultipleEmpty = new CardGroup
            {
                Level1 = new Deck<DevelopmentCard>(),
                Level2 = new Deck<DevelopmentCard>(),
                Level3 = decks.Level3,
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
                decks,
                new[]
                {
                    new ReserveFromDeckAction() { Level = 1 },
                    new ReserveFromDeckAction() { Level = 2 },
                    new ReserveFromDeckAction() { Level = 3 },
                })
                .SetName("Reserve From Deck - Zero reserved cards + non-zero decks");

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
                decks,
                new ReserveFromDeckAction[] { })
                .SetName("Reserve From Deck - Three  reserved cards + non-zero decks");


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
                decksOneEmpty,
                new[]
                {
                    new ReserveFromDeckAction() { Level = 2 },
                    new ReserveFromDeckAction() { Level = 3 },
                })
                .SetName("Reserve From Deck - Zero reserved cards + one empty deck");

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
                decksMultipleEmpty,
                new[]
                {
                    new ReserveFromDeckAction() { Level = 3 },
                })
                .SetName("Reserve From Deck - Zero reserved cards + multiple empty deck");

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
                decksOneEmpty,
                new ReserveFromDeckAction[] { })
                .SetName("Reserve From Deck - Three  reserved cards + one empty deck");
        }
    }
}
