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
    public class ReserveFromDeckExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IActionExplorer<ReserveFromDeckAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new ReserveFromDeckExplorer();
        }

        [TestCaseSource(nameof(ReserveFromDeckActionCases))]
        public void Explore_WithDifferentPlayerAndDeckSetup_ShouldReturnExpectedReserveFromDeckActions(ImmutableList<DevelopmentCard> player1Reserved, CardGroup[] cardSetup, ReserveFromDeckAction[] expectedActions)
        {
            // Arrange
            var gameState = _defaultBuilder
                .WithPlayerReservedCards(1, player1Reserved)
                .WithCurrentPlayer(1)
                .WithLevel1(cardSetup[0])
                .WithLevel2(cardSetup[1])
                .WithLevel3(cardSetup[2])
                .Build();

            // Act
            var validActions = _sut.Explore(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.That(validActions, Is.EquivalentTo(expectedActions));
        }

        private static IEnumerable<TestCaseData> ReserveFromDeckActionCases()
        {
            var defaultCards = GameFactory.GetRandomizedCardSetup();

            var emptyLevel1 = new CardGroup()
            {
                Deck = ImmutableList<DevelopmentCard>.Empty,
                Market = Cards.Level1.Take(4).ToImmutableList()
            };

            var emptyLevel2 = new CardGroup()
            {
                Deck = ImmutableList<DevelopmentCard>.Empty,
                Market = Cards.Level2.Take(4).ToImmutableList()
            };

            var emptyLevel3 = new CardGroup()
            {
                Deck = ImmutableList<DevelopmentCard>.Empty,
                Market = Cards.Level3.Take(4).ToImmutableList()
            };

            var fullCardsSetup = new CardGroup[] { defaultCards[0], defaultCards[1], defaultCards[2] };

            var level1DeckEmpty = new CardGroup[] { emptyLevel1, defaultCards[1], defaultCards[2] };

            var multipleDecksEmpty = new CardGroup[] { defaultCards[0], emptyLevel2, emptyLevel3 };

            yield return new TestCaseData(
                ImmutableList<DevelopmentCard>.Empty,
                fullCardsSetup,
                new[]
                {
                    new ReserveFromDeckAction() { Level = 1 },
                    new ReserveFromDeckAction() { Level = 2 },
                    new ReserveFromDeckAction() { Level = 3 },
                })
                .SetName("Reserve From Deck - Zero reserved cards + non-zero decks");

            yield return new TestCaseData(
                defaultCards[0].Deck.Take(3).ToImmutableList(),
                fullCardsSetup,
                new ReserveFromDeckAction[] { })
                .SetName("Reserve From Deck - Three  reserved cards + non-zero decks");


            yield return new TestCaseData(
                ImmutableList<DevelopmentCard>.Empty,
                level1DeckEmpty,
                new[]
                {
                    new ReserveFromDeckAction() { Level = 2 },
                    new ReserveFromDeckAction() { Level = 3 },
                })
                .SetName("Reserve From Deck - Zero reserved cards + one empty deck");

            yield return new TestCaseData(
                ImmutableList<DevelopmentCard>.Empty,
                multipleDecksEmpty,
                new[]
                {
                    new ReserveFromDeckAction() { Level = 1 },
                })
                .SetName("Reserve From Deck - Zero reserved cards + multiple empty deck");

            yield return new TestCaseData(
                defaultCards[0].Deck.Take(3).ToImmutableList(),
                level1DeckEmpty,
                new ReserveFromDeckAction[] { })
                .SetName("Reserve From Deck - Three  reserved cards + one empty deck");
        }
    }
}
