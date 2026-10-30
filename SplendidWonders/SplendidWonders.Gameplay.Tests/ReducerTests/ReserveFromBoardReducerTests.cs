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
            Assert.That(newState.Players[0].ReservedCards, Is.EqualTo(expectedPlayerReserved));
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
                Assert.That(newStateMarket.Count, Is.EqualTo(baseStateMarket.Count - 1));
                Assert.That(newStateDeck.Count, Is.EqualTo(0));
            }
        }

        private static IEnumerable<TestCaseData> ReserveFromBoardCardSeed0Cases()
        {
            var cardToReserve2 = Cards.Level1.First(c => c.Id == 2);
            var cardToReserve29 = Cards.Level1.First(c => c.Id == 29);
            var cardToReserve36 = Cards.Level1.First(c => c.Id == 36);
            var cardToReserve41 = Cards.Level2.First(c => c.Id == 41);
            var cardToReserve52 = Cards.Level2.First(c => c.Id == 52);
            var cardToReserve58 = Cards.Level2.First(c => c.Id == 58);
            var cardToReserve78 = Cards.Level3.First(c => c.Id == 78);
            var cardToReserve84 = Cards.Level3.First(c => c.Id == 84);

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
                new ReserveFromBoardAction() { Card = cardToReserve36 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve36 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 1 reserving #34, zero reserved, zero store, polymath available");

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
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve78 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 3 reserving #78, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve52 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, cardToReserve52 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 2 reserving #52, one reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve84 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89, cardToReserve84 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 3 reserving #84, two reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve36 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 1]),
                new List<DevelopmentCard>() { cardToReserve36 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - level 1 reserving #36, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve78 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - level 3 reserving #78, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve58 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 1]),
                new List<DevelopmentCard>() { cardToReserve58 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 2 reserving #58, zero reserved, 9-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 1]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve84 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                new List<DevelopmentCard>() { cardToReserve84 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 3 reserving #84, zero reserved, 4-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved89, alreadyReserved23 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve29 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve29 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 1 reserving #29, two reserved, 5-long store, no polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { alreadyReserved89 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { cardToReserve2 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - level 1 reserving #2, one reserved, full store, no polymath available - does not exceed");

        }
    }
}
