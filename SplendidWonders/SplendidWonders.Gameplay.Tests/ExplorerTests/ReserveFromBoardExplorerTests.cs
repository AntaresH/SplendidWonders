using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests.ExplorerTests
{
    public class ReserveFromBoardExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IActionExplorer<ReserveFromBoardAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new ReserveFromBoardExplorer();
        }


        [TestCaseSource(nameof(ReserveFromBoardActionCases))]
        public void Explore_WithDifferentPlayerAndMarketSetup_ShouldReturnExpectedReserveFromBoardActions(List<Player> playerSetup, CardGroup[] cardSetup, ReserveFromBoardAction[] expectedActions)
        {
            // Arrange
            var gameState = _defaultBuilder
                .WithCurrentPlayer(playerSetup[0])
                .WithLevel1(cardSetup[0])
                .WithLevel2(cardSetup[1])
                .WithLevel3(cardSetup[2])
                .WithPlayers(playerSetup)
                .Build();

            // Act
            var validActions = _sut.Explore(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.That(validActions, Is.EquivalentTo(expectedActions));
        }

        private static IEnumerable<TestCaseData> ReserveFromBoardActionCases()
        {
            var defaultCards = GameFactory.GetRandomizedCardSetup();

            var shortLevel1 = new CardGroup()
            {
                Deck = Cards.Level1.Skip(4).ToImmutableList(),
                Market = Cards.Level1.Take(3).ToImmutableList().Add(Cards.Empty)
            };

            var shortLevel2 = new CardGroup()
            {
                Deck = Cards.Level2.Skip(4).ToImmutableList(),
                Market = Cards.Level2.Take(3).ToImmutableList().Add(Cards.Empty)
            };

            var shortLevel3 = new CardGroup()
            {
                Deck = Cards.Level3.Skip(4).ToImmutableList(),
                Market = Cards.Level3.Take(3).ToImmutableList().Add(Cards.Empty)
            };

            var shortCardsSetup1 = new CardGroup[] { shortLevel1, defaultCards[1], defaultCards[2] };

            var shortCardsSetup2 = new CardGroup[] { defaultCards[0], shortLevel2, shortLevel3 };

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    }
                },
                defaultCards,
                new[]
                {
                    new ReserveFromBoardAction() { Card = defaultCards[0][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][3] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][4] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][3] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][4] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][3] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + full market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = defaultCards[0].Deck.Take(3).ToImmutableList(),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0, 0])
                    }
                },
                defaultCards,
                new ReserveFromBoardAction[] { })
                .SetName("Reserve From Board - Three reserved cards + full market");


            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    }
                },
                shortCardsSetup1,
                new[]
                {
                    new ReserveFromBoardAction() { Card = defaultCards[0][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][3] },
             //     new ReserveFromBoardAction() { Card = defaultCards[0][4] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][3] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][4] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][3] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + short market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    }
                },
                shortCardsSetup2,
                new[]
                {
                    new ReserveFromBoardAction() { Card = defaultCards[0][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][3] },
                    new ReserveFromBoardAction() { Card = defaultCards[0][4] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[1][3] },
                //  new ReserveFromBoardAction() { Card = defaultCards[1][4] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][1] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][2] },
                    new ReserveFromBoardAction() { Card = defaultCards[2][3] },
                //  new ReserveFromBoardAction() { Card = defaultCards[2][4] },
                })
                .SetName("Reserve From Board - Zero reserved cards + very short market");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = defaultCards[0].Deck.Take(3).ToImmutableList(),
                        Store = TokenGroup.FromArray([4, 4, 2, 0, 0, 0])
                    }
                },
                shortCardsSetup1,
                new ReserveFromBoardAction[] { })
                .SetName("Reserve From Board - Three reserved cards + short market");
        }
    }
}
