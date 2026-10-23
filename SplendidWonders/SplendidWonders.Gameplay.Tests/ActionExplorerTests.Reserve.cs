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

        private static IEnumerable<TestCaseData> ReserveFromBoardActionCases()
        {
            var level1 = new CardGroup()
            {
                Deck = Cards.Level1.Skip(4).ToImmutableList(),
                Market = Cards.Level1.Take(4).ToImmutableList()
            };

            var level2 = new CardGroup()
            {
                Deck = Cards.Level2.Skip(4).ToImmutableList(),
                Market = Cards.Level2.Take(4).ToImmutableList()
            };

            var level3 = new CardGroup()
            {
                Deck = Cards.Level3.Skip(4).ToImmutableList(),
                Market = Cards.Level3.Take(4).ToImmutableList()
            };

            var shortLevel1 = new CardGroup()
            {
                Deck = Cards.Level1.Skip(4).ToImmutableList(),
                Market = Cards.Level1.Take(3).ToImmutableList()
            };

            var shortLevel2 = new CardGroup()
            {
                Deck = Cards.Level2.Skip(4).ToImmutableList(),
                Market = Cards.Level2.Take(3).ToImmutableList()
            };

            var shortLevel3 = new CardGroup()
            {
                Deck = Cards.Level3.Skip(4).ToImmutableList(),
                Market = Cards.Level3.Take(3).ToImmutableList()
            };

            var fullCardsSetup = new CardGroup[] { level1, level2, level3 };

            var shortCardsSetup1 = new CardGroup[] { shortLevel1, level2, level3 };

            var shortCardsSetup2 = new CardGroup[] { level1, shortLevel2, shortLevel3 };

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                fullCardsSetup,
                new[]
                {
                    new ReserveFromBoardAction() { Card = level1[1] },
                    new ReserveFromBoardAction() { Card = level1[2] },
                    new ReserveFromBoardAction() { Card = level1[3] },
                    new ReserveFromBoardAction() { Card = level1[4] },
                    new ReserveFromBoardAction() { Card = level2[1] },
                    new ReserveFromBoardAction() { Card = level2[2] },
                    new ReserveFromBoardAction() { Card = level2[3] },
                    new ReserveFromBoardAction() { Card = level2[4] },
                    new ReserveFromBoardAction() { Card = level3[1] },
                    new ReserveFromBoardAction() { Card = level3[2] },
                    new ReserveFromBoardAction() { Card = level3[3] },
                    new ReserveFromBoardAction() { Card = level3[4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + full market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = level1.Deck.Take(3).ToImmutableList(),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0])
                    }
                },
                fullCardsSetup,
                new ReserveFromBoardAction[] {})
                .SetName("Reserve From Board - Three  reserved cards + full market");


            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                shortCardsSetup1,
                new[]
                {
                    new ReserveFromBoardAction() { Card = level1[1] },
                    new ReserveFromBoardAction() { Card = level1[2] },
                    new ReserveFromBoardAction() { Card = level1[3] },
             //     new ReserveFromBoardAction() { Card = level1[4] },
                    new ReserveFromBoardAction() { Card = level2[1] },
                    new ReserveFromBoardAction() { Card = level2[2] },
                    new ReserveFromBoardAction() { Card = level2[3] },
                    new ReserveFromBoardAction() { Card = level2[4] },
                    new ReserveFromBoardAction() { Card = level3[1] },
                    new ReserveFromBoardAction() { Card = level3[2] },
                    new ReserveFromBoardAction() { Card = level3[3] },
                    new ReserveFromBoardAction() { Card = level3[4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + short market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                shortCardsSetup2,
                new[]
                {
                    new ReserveFromBoardAction() { Card = level1[1] },
                    new ReserveFromBoardAction() { Card = level1[2] },
                    new ReserveFromBoardAction() { Card = level1[3] },
                    new ReserveFromBoardAction() { Card = level1[4] },
                    new ReserveFromBoardAction() { Card = level2[1] },
                    new ReserveFromBoardAction() { Card = level2[2] },
                    new ReserveFromBoardAction() { Card = level2[3] },
                //  new ReserveFromBoardAction() { Card = level2[4] },
                    new ReserveFromBoardAction() { Card = level3[1] },
                    new ReserveFromBoardAction() { Card = level3[2] },
                    new ReserveFromBoardAction() { Card = level3[3] },
                //  new ReserveFromBoardAction() { Card = level3[4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + very short market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = level1.Deck.Take(3).ToImmutableList(),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0])
                    }
                },
                shortCardsSetup1,
                new ReserveFromBoardAction[] { })
                .SetName("Reserve From Board - Three  reserved cards + short market");
        }

        private static IEnumerable<TestCaseData> ReserveFromDeckActionCases()
        {
            var level1 = new CardGroup()
            {
                Deck = Cards.Level1.Skip(4).ToImmutableList(),
                Market = Cards.Level1.Take(4).ToImmutableList()
            };

            var level2 = new CardGroup()
            {
                Deck = Cards.Level2.Skip(4).ToImmutableList(),
                Market = Cards.Level2.Take(4).ToImmutableList()
            };

            var level3 = new CardGroup()
            {
                Deck = Cards.Level3.Skip(4).ToImmutableList(),
                Market = Cards.Level3.Take(4).ToImmutableList()
            };

            var shortLevel1 = new CardGroup()
            {
                Deck = ImmutableList<DevelopmentCard>.Empty,
                Market = Cards.Level1.Take(4).ToImmutableList()
            };

            var shortLevel2 = new CardGroup()
            {
                Deck = ImmutableList<DevelopmentCard>.Empty,
                Market = Cards.Level2.Take(4).ToImmutableList()
            };

            var shortLevel3 = new CardGroup()
            {
                Deck = ImmutableList<DevelopmentCard>.Empty,
                Market = Cards.Level3.Take(4).ToImmutableList()
            };

            var fullCardsSetup = new CardGroup[] { level1, level2, level3 };

            var level1DeckEmpty = new CardGroup[] { shortLevel1, level2, level3 };

            var multipleDecksEmpty = new CardGroup[] { level1, shortLevel2, shortLevel3 };

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                fullCardsSetup,
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
                        ReservedCards = level1.Deck.Take(3).ToImmutableList(),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0])
                    }
                },
                fullCardsSetup,
                new ReserveFromDeckAction[] { })
                .SetName("Reserve From Deck - Three  reserved cards + non-zero decks");


            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                level1DeckEmpty,
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
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    }
                },
                multipleDecksEmpty,
                new[]
                {
                    new ReserveFromDeckAction() { Level = 1 },
                })
                .SetName("Reserve From Deck - Zero reserved cards + multiple empty deck");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = level1.Deck.Take(3).ToImmutableList(),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0])
                    }
                },
                level1DeckEmpty,
                new ReserveFromDeckAction[] { })
                .SetName("Reserve From Deck - Three  reserved cards + one empty deck");
        }
    }
}
