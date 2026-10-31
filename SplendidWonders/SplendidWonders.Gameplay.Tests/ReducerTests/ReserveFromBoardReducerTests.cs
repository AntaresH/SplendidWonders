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
        public void Reduce_CardSeed0WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupply, TokenGroup player1Store, ImmutableList<DevelopmentCard> player1Reserved,
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

        [TestCaseSource(nameof(ReserveFromBoardCardSeed23456Cases))]
        public void Reduce_CardSeed23456WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupply, TokenGroup player1Store, ImmutableList<DevelopmentCard> player1Reserved,
            ReserveFromBoardAction actionToApply,
            TokenGroup expectedTokenSupply, TokenGroup expectedPlayerStore, ImmutableList<DevelopmentCard> expectedPlayerReserved, int expectedCurrentPlayerId)
        {
            int testSeed = 23456;

            var seededCardSetup = GameFactory.GetRandomizedCardSetup(testSeed);

            // Arrange
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

            AssertCardReplacement(baseState, newState, actionToApply);
        }

        [TestCaseSource(nameof(ReserveFromBoardShortDecksCases))]
        public void Reduce_ShortDecksWithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupply, TokenGroup player1Store, ImmutableList<DevelopmentCard> player1Reserved,
            ReserveFromBoardAction actionToApply,
            TokenGroup expectedTokenSupply, TokenGroup expectedPlayerStore, ImmutableList<DevelopmentCard> expectedPlayerReserved, int expectedCurrentPlayerId)
        {
            int testSeed = 34567;

            var seededCardSetup = GameFactory.GetRandomizedCardSetup(testSeed);

            var shortLevel1Market = seededCardSetup[0].Market.Take(3).ToImmutableList().Add(Cards.Empty);
            var level1 = new CardGroup() { Deck = ImmutableList<DevelopmentCard>.Empty, Market = shortLevel1Market };
            var level2 = new CardGroup() { Deck = ImmutableList<DevelopmentCard>.Empty, Market = seededCardSetup[1].Market };
            var level3 = seededCardSetup[2];

            // Arrange
            var baseState = _defaultBuilder
                .WithLevel1(level1)
                .WithLevel2(level2)
                .WithLevel3(level3)
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
                .SetName("Reserve From Board - Card Setup #0 - level 1 reserving #2, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve3 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 1 reserving #3, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve41 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve41 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 2 reserving #41, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve72 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve72 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 3 reserving #72, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve43 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, cardToReserve43 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 2 reserving #43, one reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve72 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved23, alreadyReserved89, cardToReserve72 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 3 reserving #72, two reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve3 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 1]),
                new List<DevelopmentCard>() { cardToReserve3 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - Card Setup #0 - level 1 reserving #3, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve72 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve72 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - Card Setup #0 - level 3 reserving #72, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve44 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 1]),
                new List<DevelopmentCard>() { cardToReserve44 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 2 reserving #44, zero reserved, 9-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 1]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve73 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                new List<DevelopmentCard>() { cardToReserve73 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 3 reserving #73, zero reserved, 4-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved89, alreadyReserved23 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve4 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve4, alreadyReserved23, alreadyReserved89 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 1 reserving #4, two reserved, 5-long store, no polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { alreadyReserved89 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve2 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { cardToReserve2, alreadyReserved89 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #0 - level 1 reserving #2, one reserved, full store, no polymath available - does not exceed");
        }
        private static IEnumerable<TestCaseData> ReserveFromBoardCardSeed23456Cases()
        {
            // Level 1
            //+ [0] { DevelopmentCard { Id = 13, Name = Sírkamra, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 0, Green = 0, Brown = 0, Red = 2, Polymath = 0 }, Bonus = Blue } }
            //+ [1] { DevelopmentCard { Id = 4, Name = Kikötő, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 2, Red = 1, Polymath = 0 }, Bonus = Yellow } }
            //+ [2] { DevelopmentCard { Id = 19, Name = Könyvtár, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 3, Green = 1, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [3] { DevelopmentCard { Id = 27, Name = Fűrésztelep, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 2, Green = 1, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Brown } }

            // Level 2
            //+ [0] { DevelopmentCard { Id = 68, Name = Ostromszakértelem, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 5, Brown = 3, Red = 0, Polymath = 0 }, Bonus = Red } }
            //+ [1] { DevelopmentCard { Id = 63, Name = Útrendszer, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 0, Red = 5, Polymath = 0 }, Bonus = Brown } }
            //+ [2] { DevelopmentCard { Id = 44, Name = Pénzverés, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 5, Red = 0, Polymath = 0 }, Bonus = Yellow } }
            //+ [3] { DevelopmentCard { Id = 46, Name = Törvénykönyv, Level = 2, VictoryPoints = 1, Cost = TokenGroup { Yellow = 2, Blue = 3, Green = 0, Brown = 3, Red = 0, Polymath = 0 }, Bonus = Yellow } }

            // Level 3
            //+ [0] { DevelopmentCard { Id = 78, Name = Knósszoszi Palota, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 6, Blue = 3, Green = 0, Brown = 0, Red = 3, Polymath = 0 }, Bonus = Blue } }
            //+ [1] { DevelopmentCard { Id = 81, Name = Nalanda Kolostor, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 0, Blue = 7, Green = 0, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [2] { DevelopmentCard { Id = 72, Name = Perszepolisz, Level = 3, VictoryPoints = 5, Cost = TokenGroup { Yellow = 3, Blue = 0, Green = 0, Brown = 0, Red = 7, Polymath = 0 }, Bonus = Yellow } }
            //+ [3] { DevelopmentCard { Id = 90, Name = Olümpiai Zeusz Szobor, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 7, Red = 0, Polymath = 0 }, Bonus = Red } }

            var cardToReserve13 = Cards.Level1.First(c => c.Id == 13);
            var cardToReserve19 = Cards.Level1.First(c => c.Id == 19);
            var cardToReserve27 = Cards.Level1.First(c => c.Id == 27);
            var cardToReserve46 = Cards.Level2.First(c => c.Id == 46);
            var cardToReserve63 = Cards.Level2.First(c => c.Id == 63);
            var cardToReserve68 = Cards.Level2.First(c => c.Id == 68);
            var cardToReserve78 = Cards.Level3.First(c => c.Id == 78);
            var cardToReserve81 = Cards.Level3.First(c => c.Id == 81);

            var alreadyReserved33 = Cards.Level1.First(c => c.Id == 33);
            var alreadyReserved76 = Cards.Level3.First(c => c.Id == 76);

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve27 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve27 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 1 reserving #27, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve13 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve13 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 1 reserving #13, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve46 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve46 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 2 reserving #46, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve78 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 3 reserving #78, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved33 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve63 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved33, cardToReserve63 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 2 reserving #63, one reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved33, alreadyReserved76 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved33, alreadyReserved76, cardToReserve78 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 3 reserving #78, two reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve13 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 1]),
                new List<DevelopmentCard>() { cardToReserve13 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - Card Setup #23456 - level 1 reserving #13, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve78 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - Card Setup #23456 - level 3 reserving #72, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve68 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 1]),
                new List<DevelopmentCard>() { cardToReserve68 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 2 reserving #68, zero reserved, 9-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 1]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve81 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                new List<DevelopmentCard>() { cardToReserve81 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 3 reserving #81, zero reserved, 4-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved76, alreadyReserved33 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve19 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve19, alreadyReserved33, alreadyReserved76 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 1 reserving #19, two reserved, 5-long store, no polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { alreadyReserved76 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve27 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { cardToReserve27, alreadyReserved76 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Card Setup #23456 - level 1 reserving #27, one reserved, full store, no polymath available - does not exceed");
        }
        private static IEnumerable<TestCaseData> ReserveFromBoardShortDecksCases()
        {
            // Level 1
            //+ [0] { DevelopmentCard { Id = 40, Name = Városfal, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 2, Green = 1, Brown = 1, Red = 0, Polymath = 0 }, Bonus = Red } }
            //+ [1] { DevelopmentCard { Id = 29, Name = Kőfejtő, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 1, Green = 1, Brown = 0, Red = 1, Polymath = 0 }, Bonus = Brown } }
            //+ [2] { DevelopmentCard { Id = 19, Name = Könyvtár, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 3, Green = 1, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [3] { DevelopmentCard { Id = 38, Name = Laktanya, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 2, Brown = 1, Red = 0, Polymath = 0 }, Bonus = Red } }

            // Level 2
            //+ [0] { DevelopmentCard { Id = 56, Name = Matematika, Level = 2, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 6, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [1] { DevelopmentCard { Id = 55, Name = Csillagászat, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 4, Blue = 2, Green = 0, Brown = 0, Red = 1, Polymath = 0 }, Bonus = Green } }
            //+ [2] { DevelopmentCard { Id = 48, Name = Iparművészet, Level = 2, VictoryPoints = 1, Cost = TokenGroup { Yellow = 0, Blue = 2, Green = 3, Brown = 3, Red = 0, Polymath = 0 }, Bonus = Blue } }
            //+ [3] { DevelopmentCard { Id = 53, Name = Alkímia, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 5, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }

            // Level 3
            //+ [0] { DevelopmentCard { Id = 89, Name = Kolosszeum, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 3, Brown = 6, Red = 3, Polymath = 0 }, Bonus = Red } }
            //+ [1] { DevelopmentCard { Id = 78, Name = Knósszoszi Palota, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 6, Blue = 3, Green = 0, Brown = 0, Red = 3, Polymath = 0 }, Bonus = Blue } }
            //+ [2] { DevelopmentCard { Id = 86, Name = Pantheon, Level = 3, VictoryPoints = 3, Cost = TokenGroup { Yellow = 3, Blue = 5, Green = 3, Brown = 0, Red = 2, Polymath = 0 }, Bonus = Brown } }
            //+ [3] { DevelopmentCard { Id = 73, Name = Petra, Level = 3, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 2, Green = 3, Brown = 5, Red = 3, Polymath = 0 }, Bonus = Yellow } }

            var cardToReserve40 = Cards.Level1.First(c => c.Id == 40);
            var cardToReserve29 = Cards.Level1.First(c => c.Id == 29);
            var cardToReserve19 = Cards.Level1.First(c => c.Id == 19);
            var cardToReserve56 = Cards.Level2.First(c => c.Id == 56);
            var cardToReserve55 = Cards.Level2.First(c => c.Id == 55);
            var cardToReserve48 = Cards.Level2.First(c => c.Id == 48);
            var cardToReserve78 = Cards.Level3.First(c => c.Id == 78);
            var cardToReserve89 = Cards.Level3.First(c => c.Id == 89);

            var alreadyReserved25 = Cards.Level1.First(c => c.Id == 25);
            var alreadyReserved81 = Cards.Level3.First(c => c.Id == 81);

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve29 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve29 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 1 reserving #29, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve29 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve29 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 1 reserving #29, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve56 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve56 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 2 reserving #56, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve78 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 3 reserving #78, zero reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved25 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve55 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved25, cardToReserve55 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 2 reserving #25, one reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new List<DevelopmentCard>() { alreadyReserved25, alreadyReserved81 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved25, alreadyReserved81, cardToReserve78 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 3 reserving #78, two reserved, zero store, polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve29 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([3, 2, 0, 3, 2, 1]),
                new List<DevelopmentCard>() { cardToReserve29 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - Short Decks #34567 - level 1 reserving #29, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve78 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([6, 4, 0, 0, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve78 }.ToImmutableList(),
                1)
                .SetName("Reserve From Board - Short Decks #34567 - level 3 reserving #78, zero reserved, full store, polymath available - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 0]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve48 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([1, 3, 3, 1, 1, 1]),
                new List<DevelopmentCard>() { cardToReserve48 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 2 reserving #48, zero reserved, 9-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 1]),
                ImmutableList<DevelopmentCard>.Empty,
                new ReserveFromBoardAction() { Card = cardToReserve89 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 4]),
                TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                new List<DevelopmentCard>() { cardToReserve89 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 3 reserving #89, zero reserved, 4-long store, polymath available - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { alreadyReserved81, alreadyReserved25 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve19 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 0, 1, 1, 0, 1]),
                new List<DevelopmentCard>() { cardToReserve19, alreadyReserved25, alreadyReserved81 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 1 reserving #19, two reserved, 5-long store, no polymath available");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { alreadyReserved81 }.ToImmutableList(),
                new ReserveFromBoardAction() { Card = cardToReserve40 },
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                TokenGroup.FromArray([2, 3, 3, 1, 1, 0]),
                new List<DevelopmentCard>() { cardToReserve40, alreadyReserved81 }.ToImmutableList(),
                2)
                .SetName("Reserve From Board - Short Decks #34567 - level 1 reserving #40, one reserved, full store, no polymath available - does not exceed");
        }
    }
}
