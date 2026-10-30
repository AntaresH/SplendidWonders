using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests.ReducerTests
{
    public class ReserveFromBoardReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<ReserveFromBoardAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new ReserveFromBoardReducer();
        }

        [TestCaseSource(nameof(ReserveFromBoardCardSeed0Cases))]
        public void Reduce_WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupply, TokenGroup player1Store, ImmutableList<DevelopmentCard> player1Reserved,
            ReserveFromBoardAction actionToApply, 
            TokenGroup expectedTokenSupply, TokenGroup expectedPlayerStore, ImmutableList<DevelopmentCard> expectedPlayerReserved, int expectedCurrentPlayerId)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(1)
                .WithPlayerStore(1, player1Store)
                .WithPlayerReservedCards(1, player1Reserved)
                .WithTokenSupply(tokenSupply)
                .Build();

            // Act
            var newState = _sut.Reduce(baseState, actionToApply);

            // Assert
            Assert.IsNotNull(newState);
            Assert.That(newState.TokenSupply, Is.EqualTo(expectedTokenSupply));
            Assert.That(newState.Players[0].Store, Is.EqualTo(expectedPlayerStore));
            Assert.That(newState.Players[0].ReservedCards, Is.EquivalentTo(expectedPlayerReserved));
            Assert.That(newState.CurrentPlayer.Id, Is.EqualTo(expectedCurrentPlayerId));

            AssertCardReplacement(baseState, newState, actionToApply);
        }

        private void AssertCardReplacement(GameState baseState, GameState newState, ReserveFromBoardAction gameAction)
        {
            int level = gameAction.Card.Id < 41 ? 1 : gameAction.Card.Id < 71 ? 2 : 3;

            var baseStateMarket = baseState[level].Market;
            var newStateMarket = newState[level].Market;
            var baseStateDeck = baseState[level].Deck;
            var newStateDeck = newState[level].Deck;

            if (baseStateDeck.Any())
            {
                Assert.That(newStateMarket.Count, Is.EqualTo(4));
                Assert.That(newStateDeck.Count, Is.EqualTo(baseStateDeck.Count - 1));
            }
            else
            {
                Assert.That(newStateMarket.Count(c => c != Cards.Empty), Is.EqualTo(baseStateMarket.Count - 1));
                Assert.That(newStateDeck.Count, Is.EqualTo(0));
            }
        }

        private static IEnumerable<TestCaseData> ReserveFromBoardCardSeed0Cases()
        {
            var cardToReserve2 = Cards.Level1.First(c => c.Id == 2);
            var cardToReserve3 = Cards.Level1.First(c => c.Id == 3);
            var cardToReserve4 = Cards.Level1.First(c => c.Id == 4);
            var cardToReserve41 = Cards.Level2.First(c => c.Id == 41);
            var cardToReserve43 = Cards.Level2.First(c => c.Id == 43);
            var cardToReserve44 = Cards.Level2.First(c => c.Id == 44);
            var cardToReserve72 = Cards.Level3.First(c => c.Id == 72);
            var cardToReserve73 = Cards.Level3.First(c => c.Id == 73);

            var alreadyReserved23 = Cards.Level1.First(c => c.Id == 23);
            var alreadyReserved89 = Cards.Level3.First(c => c.Id == 89);

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve2 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 1 reserving #2, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve3 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 1 reserving #3, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve41 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve41 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 2 reserving #41, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve72 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve72 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 3 reserving #72, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve43 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, cardToReserve43 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 2 reserving #43, one reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve72 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89, cardToReserve72 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 3 reserving #72, two reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 1]),
                new List<DevelopmentCard>() { cardToReserve3 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - level 1 reserving #3, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve72 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve72 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - level 3 reserving #72, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve44 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 1]),
                new List<DevelopmentCard>() { cardToReserve44 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 2 reserving #44, zero reserved, 9-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 1]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve73 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                new List<DevelopmentCard>() { cardToReserve73 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 3 reserving #73, zero reserved, 4-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved89, alreadyReserved23 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve4 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve4, alreadyReserved23, alreadyReserved89 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 1 reserving #4, two reserved, 5-long store, no polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { alreadyReserved89 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { cardToReserve2, alreadyReserved89 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 1 reserving #2, one reserved, full store, no polymath available - does not exceed");

        }
    }
}
