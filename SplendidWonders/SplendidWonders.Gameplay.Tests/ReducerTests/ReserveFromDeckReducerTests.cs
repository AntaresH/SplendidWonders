using Newtonsoft.Json.Linq;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Xml.Linq;

namespace SplendidWonders.Gameplay.Tests.ReducerTests
{
    public class ReserveFromDeckReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<ReserveFromDeckAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new ReserveFromDeckReducer();
        }

        [TestCaseSource(nameof(ReserveFromDeckCardSeed0Cases))]
        public void Reduce_WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupply, TokenGroup player1Store, ImmutableList<DevelopmentCard> player1Reserved,
            ReserveFromDeckAction actionToApply, 
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
            Assert.That(newState[actionToApply.Level].Deck.Count, Is.EqualTo(baseState[actionToApply.Level].Deck.Count - 1));
        }

        [TestCaseSource(nameof(ReserveFromDeckCardSeed23456Cases))]
        public void Reduce_CardSeed23456WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupply, TokenGroup player1Store, ImmutableList<DevelopmentCard> player1Reserved,
            ReserveFromDeckAction actionToApply,
            TokenGroup expectedTokenSupply, TokenGroup expectedPlayerStore, ImmutableList<DevelopmentCard> expectedPlayerReserved, int expectedCurrentPlayerId)
        {
            // Arrange
            int testSeed = 23456;
            var seededCardSetup = GameFactory.GetRandomizedCardSetup(testSeed);

            var baseState = _defaultBuilder
                .WithLevel1(seededCardSetup[0])
                .WithLevel2(seededCardSetup[1])
                .WithLevel3(seededCardSetup[2])
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
            Assert.That(newState[actionToApply.Level].Deck.Count, Is.EqualTo(baseState[actionToApply.Level].Deck.Count - 1));
        }

        private static IEnumerable<TestCaseData> ReserveFromDeckCardSeed0Cases()
        {
            var alreadyReserved23 = Cards.Level1.First(c => c.Id == 23);
            var alreadyReserved89 = Cards.Level3.First(c => c.Id == 89);

            var level1DeckTopCard = Cards.Level1.First(c => c.Id == 5);
            var level2DeckTopCard = Cards.Level2.First(c => c.Id == 45);
            var level3DeckTopCard = Cards.Level3.First(c => c.Id == 75);

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 1 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { level1DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 1, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { level2DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 2, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { level3DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 3, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23 }.ToImmutableList(),
                new ReserveFromDeckAction() { Level = 2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, level2DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 2, one reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89 }.ToImmutableList(),
                new ReserveFromDeckAction() { Level = 3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89, level3DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 3, two reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 1 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 1]),
                new List<DevelopmentCard>() { level1DeckTopCard }.ToImmutableList(),
                1)
                .SetName("Reserve From Deck - Card Setup #0 - level 1, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { level3DeckTopCard }.ToImmutableList(),
                1)
                .SetName("Reserve From Deck - Card Setup #0 - level 3, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 1]),
                new List<DevelopmentCard>() { level3DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 2, zero reserved, 9-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 1]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                new List<DevelopmentCard>() { level3DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 3, zero reserved, 4-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved89, alreadyReserved23 }.ToImmutableList(),
                new ReserveFromDeckAction() { Level = 1 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { level1DeckTopCard, alreadyReserved23, alreadyReserved89 }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 1, two reserved, 5-long store, no polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { alreadyReserved89 }.ToImmutableList(),
                new ReserveFromDeckAction() { Level = 1 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { level1DeckTopCard, alreadyReserved89 }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #0 - level 1 reserving #2, one reserved, full store, no polymath available - does not exceed");
        }

        private static IEnumerable<TestCaseData> ReserveFromDeckCardSeed23456Cases()
        {
            var level1DeckTopCard = Cards.Level1.First(c => c.Id == 30);
            var level2DeckTopCard = Cards.Level2.First(c => c.Id == 42);
            var level3DeckTopCard = Cards.Level3.First(c => c.Id == 84);

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 1 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { level1DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #23456 - level 1");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { level2DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #23456 - level 2");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromDeckAction() { Level = 3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { level3DeckTopCard }.ToImmutableList(),
                2)
                .SetName("Reserve From Deck - Card Setup #23456 - level 3");
        }
    }
}
