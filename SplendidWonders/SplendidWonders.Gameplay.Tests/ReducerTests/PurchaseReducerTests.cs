using Newtonsoft.Json.Linq;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using SplendidWonders.Gameplay.Tests.TestDataModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Xml.Linq;

namespace SplendidWonders.Gameplay.Tests.ReducerTests
{
    public class PurchaseReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<PurchaseAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new PurchaseReducer();
        }

        [TestCaseSource(nameof(PurchaseCardSeed0Cases))]
        public void Reduce_CardSeed0_WithDifferentPurchaseScenarios_ShouldReturnExpectedPurchaseResults(
            ReducerTestData input, PurchaseAction actionToApply, ReducerTestData expected)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(input.ActivePlayerId)
                .WithPlayerStore(input.ActivePlayerId, input.PlayerStore)
                .WithPlayerReservedCards(input.ActivePlayerId, input.PlayerReservedCards)
                .WithPlayerTableau(input.ActivePlayerId, input.PlayerTableau)
                .WithTokenSupply(input.TokenSupply)
                .Build();

            // Act
            var newState = _sut.Reduce(baseState, actionToApply);

            // Assert
            Assert.IsNotNull(newState);
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(expected.ActivePlayerId));
            Assert.That(newState.TokenSupply, Is.EqualTo(expected.TokenSupply));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Store, Is.EqualTo(expected.PlayerStore));
            Assert.That(newState.Players[input.ActivePlayerId - 1].ReservedCards, Is.EquivalentTo(expected.PlayerReservedCards));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Tableau, Is.EquivalentTo(expected.PlayerTableau));

            AssertCardReplacement(baseState, newState, actionToApply);
        }

        [TestCaseSource(nameof(PurchaseCardSeed11111Tableau33104Cases))]
        public void Reduce_CardSeed11111_Tableau33104_WithDifferentPurchaseScenarios_ShouldReturnExpectedPurchaseResults(
            ReducerTestData input, PurchaseAction actionToApply, ReducerTestData expected)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithLevel1(input.Level1)
                .WithLevel2(input.Level2)
                .WithLevel3(input.Level3)
                .WithCurrentPlayer(input.ActivePlayerId)
                .WithPlayerStore(input.ActivePlayerId, input.PlayerStore)
                .WithPlayerReservedCards(input.ActivePlayerId, input.PlayerReservedCards)
                .WithPlayerTableau(input.ActivePlayerId, input.PlayerTableau)
                .WithTokenSupply(input.TokenSupply)
                .Build();

            // Act
            var newState = _sut.Reduce(baseState, actionToApply);

            // Assert
            Assert.IsNotNull(newState);
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(expected.ActivePlayerId));
            Assert.That(newState.TokenSupply, Is.EqualTo(expected.TokenSupply));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Store, Is.EqualTo(expected.PlayerStore));
            Assert.That(newState.Players[input.ActivePlayerId - 1].ReservedCards, Is.EquivalentTo(expected.PlayerReservedCards));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Tableau, Is.EquivalentTo(expected.PlayerTableau));

            AssertCardReplacement(baseState, newState, actionToApply);
        }

        [TestCaseSource(nameof(LeaderEligibilityCases_Singular))]
        [TestCaseSource(nameof(LeaderEligibilityCases_Multiple))]
        public void Reduce_LeaderEligibilityCases_WithDifferentPurchaseScenarios_ShouldReturnExpectedPurchaseResults(
            ReducerTestData input, PurchaseAction actionToApply, ReducerTestData expected)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(input.ActivePlayerId)
                .WithLeaders(input.AvailableLeaders)
                .WithPlayerAcquiredLeaders(input.ActivePlayerId, input.PlayerAcquiredLeaders)
                .WithPlayerReservedCards(input.ActivePlayerId, input.PlayerReservedCards)
                .WithPlayerTableau(input.ActivePlayerId, input.PlayerTableau)
                .Build();

            // Act
            var newState = _sut.Reduce(baseState, actionToApply);

            // Assert
            Assert.IsNotNull(newState);
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(expected.ActivePlayerId));
            Assert.That(newState.Leaders, Is.EquivalentTo(expected.AvailableLeaders));
            Assert.That(newState.Players[input.ActivePlayerId - 1].ReservedCards, Is.EquivalentTo(expected.PlayerReservedCards));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Tableau, Is.EquivalentTo(expected.PlayerTableau));
            Assert.That(newState.Players[input.ActivePlayerId - 1].AcquiredLeaders, Is.EquivalentTo(expected.PlayerAcquiredLeaders));

            AssertCardReplacement(baseState, newState, actionToApply);
        }
        private void AssertCardReplacement(GameState baseState, GameState newState, PurchaseAction gameAction)
        {
            int level = gameAction.Card.Id < 41 ? 1 : gameAction.Card.Id < 71 ? 2 : 3;

            var baseStateMarket = baseState[level].Market;
            var newStateMarket = newState[level].Market;
            var baseStateDeck = baseState[level].Deck;
            var newStateDeck = newState[level].Deck;

            if (baseStateDeck.Any())
            {
                Assert.That(newStateMarket.Count, Is.EqualTo(4));
            }
            else
            {
                Assert.That(newStateMarket.Count(c => c != Cards.Empty), Is.EqualTo(baseStateMarket.Count(c => c != Cards.Empty) - 1));
                Assert.That(newStateDeck.Count, Is.EqualTo(0));
            }
        }

        private static IEnumerable<TestCaseData> PurchaseCardSeed0Cases()
        {
            var cardToBuy2 = Cards.Level1.First(c => c.Id == 2);
            var cardToBuy3 = Cards.Level1.First(c => c.Id == 3);
            var cardToBuy4 = Cards.Level1.First(c => c.Id == 4);
            var cardToBuy41 = Cards.Level2.First(c => c.Id == 41);
            var cardToBuy42 = Cards.Level2.First(c => c.Id == 42);
            var cardToBuy43 = Cards.Level2.First(c => c.Id == 43);
            var cardToBuy44 = Cards.Level2.First(c => c.Id == 44);
            var cardToBuy72 = Cards.Level3.First(c => c.Id == 72);
            var cardToBuy73 = Cards.Level3.First(c => c.Id == 73);
            var cardToBuy74 = Cards.Level3.First(c => c.Id == 74);

            var alreadyReserved23 = Cards.Level1.First(c => c.Id == 23);
            var alreadyReserved58 = Cards.Level2.First(c => c.Id == 58);
            var alreadyReserved64 = Cards.Level2.First(c => c.Id == 64);
            var alreadyReserved89 = Cards.Level3.First(c => c.Id == 89);

            yield return new TestCaseData(
                new ReducerTestData() { 
                    ActivePlayerId = 1, 
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty, 
                    PlayerStore = TokenGroup.FromArray([0, 3, 0, 0, 0, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([1, 2, 4, 2, 0, 5]),
                },
                new PurchaseAction() { Card = cardToBuy2 },
                new ReducerTestData() {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = [cardToBuy2],
                    TokenSupply = TokenGroup.FromArray([1, 5, 4, 2, 0, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 1 purchasing #2");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 2, 1, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                },
                new PurchaseAction() { Card = cardToBuy4 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = [cardToBuy4],
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 6, 5, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 1 purchasing #4");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 1, 2, 5, 0, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([3, 3, 0, 0, 1, 5]),
                },
                new PurchaseAction() { Card = cardToBuy44 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 1, 2, 0, 0, 0]),
                    PlayerTableau = [cardToBuy44],
                    TokenSupply = TokenGroup.FromArray([3, 3, 0, 5, 1, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 2 purchasing #44");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 2, 0, 7, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 1, 4, 2, 0, 5]),
                },
                new PurchaseAction() { Card = cardToBuy74 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]),
                    PlayerTableau = [cardToBuy74],
                    TokenSupply = TokenGroup.FromArray([2, 1, 4, 2, 7, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 3 purchasing #74");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([3, 2, 1, 3, 1, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 2, 2, 5]),
                },
                new PurchaseAction() { Card = cardToBuy3 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 1, 1, 3, 0, 0]),
                    PlayerTableau = [cardToBuy3],
                    TokenSupply = TokenGroup.FromArray([5, 3, 2, 2, 3, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 1 purchasing #3, much more tokens");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([5, 2, 1, 0, 1, 1]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([0, 1, 2, 4, 2, 4]),
                },
                new PurchaseAction() { Card = cardToBuy42 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 2, 1, 0, 1, 0]),
                    PlayerTableau = [cardToBuy42],
                    TokenSupply = TokenGroup.FromArray([5, 1, 2, 4, 2, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 2 purchasing #42, polymath needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 0, 0, 0, 5, 3]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 4, 3, 3, 0, 0]),
                },
                new PurchaseAction() { Card = cardToBuy72 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = [cardToBuy72],
                    TokenSupply = TokenGroup.FromArray([4, 4, 3, 3, 5, 3]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 2 purchasing #72, multiple polymaths needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved23],
                    PlayerStore = TokenGroup.FromArray([2, 3, 1, 2, 2, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([4, 0, 1, 1, 1, 5]),
                },
                new PurchaseAction() { Card = alreadyReserved23 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 2, 1, 0, 0, 0]),
                    PlayerTableau = [alreadyReserved23],
                    TokenSupply = TokenGroup.FromArray([4, 1, 1, 3, 3, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #23");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved58],
                    PlayerStore = TokenGroup.FromArray([2, 3, 0, 0, 1, 1]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([4, 4, 1, 1, 1, 4]),
                },
                new PurchaseAction() { Card = alreadyReserved58 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = [alreadyReserved58],
                    TokenSupply = TokenGroup.FromArray([6, 7, 1, 1, 2, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #58, polymath needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = [alreadyReserved58, alreadyReserved64],
                    PlayerStore = TokenGroup.FromArray([1, 3, 1, 2, 3, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([4, 2, 4, 2, 1, 5]),
                },
                new PurchaseAction() { Card = alreadyReserved64 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved58],
                    PlayerStore = TokenGroup.FromArray([1, 0, 1, 0, 0, 0]),
                    PlayerTableau = [alreadyReserved64],
                    TokenSupply = TokenGroup.FromArray([4, 5, 4, 4, 4, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #64 from two reserved cards");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = [alreadyReserved23, alreadyReserved58, alreadyReserved89],
                    PlayerStore = TokenGroup.FromArray([1, 2, 1, 3, 3, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([4, 4, 1, 1, 1, 4]),
                },
                new PurchaseAction() { Card = alreadyReserved23 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved58, alreadyReserved89],
                    PlayerStore = TokenGroup.FromArray([1, 1, 1, 1, 1, 0]),
                    PlayerTableau = [alreadyReserved23],
                    TokenSupply = TokenGroup.FromArray([4, 5, 1, 3, 3, 4]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #23 from three reserved cards");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved58, alreadyReserved89],
                    PlayerStore = TokenGroup.FromArray([2, 1, 2, 0, 2, 2]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([3, 3, 3, 3, 3, 3]),
                },
                new PurchaseAction() { Card = alreadyReserved58 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = [alreadyReserved89],
                    PlayerStore = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]),
                    PlayerTableau = [alreadyReserved58],
                    TokenSupply = TokenGroup.FromArray([5, 4, 3, 3, 5, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #58 from two reserved cards, multiple polymaths needed");
        }

        private static IEnumerable<TestCaseData> PurchaseCardSeed11111Tableau33104Cases()
        {
            int testSeed = 11111;
            var randomizedCardSetup = GameFactory.GetRandomizedCardSetup(testSeed);

            // Level 1
            //+ [0] { DevelopmentCard { Id = 8, Name = Városháza, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 4, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Yellow } }          
            //+ [1] { DevelopmentCard { Id = 14, Name = Szentély, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 2, Brown = 0, Red = 2, Polymath = 0 }, Bonus = Blue } }
            //+ [2] { DevelopmentCard { Id = 19, Name = Könyvtár, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 3, Green = 1, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [3] { DevelopmentCard { Id = 40, Name = Városfal, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 2, Green = 1, Brown = 1, Red = 0, Polymath = 0 }, Bonus = Red } }

            // Level 2
            //+ [0] { DevelopmentCard { Id = 62, Name = Mérnökség, Level = 2, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 6, Red = 0, Polymath = 0 }, Bonus = Brown } }
            //+ [1] { DevelopmentCard { Id = 63, Name = Útrendszer, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 0, Red = 5, Polymath = 0 }, Bonus = Brown } }
            //+ [2] { DevelopmentCard { Id = 50, Name = Képzőművészet, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 5, Blue = 3, Green = 0, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Blue } }
            //+ [3] { DevelopmentCard { Id = 60, Name = Hídépítés, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 3, Blue = 0, Green = 0, Brown = 0, Red = 5, Polymath = 0 }, Bonus = Brown } }

            // Level 3
            //+ [0] { DevelopmentCard { Id = 85, Name = Hagia Sophia, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 0, Blue = 3, Green = 6, Brown = 3, Red = 0, Polymath = 0 }, Bonus = Brown } }
            //+ [1] { DevelopmentCard { Id = 72, Name = Perszepolisz, Level = 3, VictoryPoints = 5, Cost = TokenGroup { Yellow = 3, Blue = 0, Green = 0, Brown = 0, Red = 7, Polymath = 0 }, Bonus = Yellow } }
            //+ [2] { DevelopmentCard { Id = 80, Name = Gundésápur Egyetem, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 3, Blue = 6, Green = 3, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [3] { DevelopmentCard { Id = 88, Name = Kínai Nagy Fal, Level = 3, VictoryPoints = 5, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 7, Red = 3, Polymath = 0 }, Bonus = Red } }

            var playerTableau33104 = new List<DevelopmentCard>();

            playerTableau33104.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(3).ToList());
            playerTableau33104.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(3).ToList());
            playerTableau33104.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(1).ToList());
            playerTableau33104.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Red).Take(4).ToList());

            var cardToBuy8 = Cards.Level1.First(c => c.Id == 8);
            var cardToBuy14 = Cards.Level1.First(c => c.Id == 14);
            var cardToBuy19 = Cards.Level1.First(c => c.Id == 19);
            var cardToBuy60 = Cards.Level2.First(c => c.Id == 60);
            var cardToBuy62 = Cards.Level2.First(c => c.Id == 62);
            var cardToBuy72 = Cards.Level3.First(c => c.Id == 72);
            var cardToBuy80 = Cards.Level3.First(c => c.Id == 80);
            var cardToBuy85 = Cards.Level3.First(c => c.Id == 85);
            var cardToBuy88 = Cards.Level3.First(c => c.Id == 88);

            var alreadyReserved35 = Cards.Level1.First(c => c.Id == 35);
            var alreadyReserved49 = Cards.Level2.First(c => c.Id == 49);
            var alreadyReserved76 = Cards.Level3.First(c => c.Id == 76);
            var alreadyReserved84 = Cards.Level3.First(c => c.Id == 84);

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 3, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([1, 2, 2, 2, 0, 5]),
                },
                new PurchaseAction() { Card = cardToBuy8 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy8),
                    TokenSupply = TokenGroup.FromArray([1, 2, 5, 2, 0, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 1 purchasing #8");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 1, 0, 2, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                },
                new PurchaseAction() { Card = cardToBuy14 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy14),
                    TokenSupply = TokenGroup.FromArray([4, 4, 5, 4, 4, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 1 purchasing #14");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 0, 2, 3, 1, 1]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                },
                new PurchaseAction() { Card = cardToBuy19 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 0, 2, 3, 1, 1]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy19),
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 1 purchasing #19");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 1, 2, 6, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([3, 3, 0, 0, 1, 5]),
                },
                new PurchaseAction() { Card = cardToBuy62 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 1, 2, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy62),
                    TokenSupply = TokenGroup.FromArray([3, 3, 0, 6, 1, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 2 purchasing #62");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 2, 7, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([2, 1, 4, 0, 0, 5]),
                },
                new PurchaseAction() { Card = cardToBuy88 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy88),
                    TokenSupply = TokenGroup.FromArray([2, 1, 4, 7, 0, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 3 purchasing #88");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 2, 2, 4, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 2, 2, 5]),
                },
                new PurchaseAction() { Card = cardToBuy14 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 2, 1, 4, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy14),
                    TokenSupply = TokenGroup.FromArray([2, 2, 3, 2, 2, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 1 purchasing #14, much more tokens");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 2, 1, 0, 0, 1]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([0, 1, 2, 4, 2, 4]),
                },
                new PurchaseAction() { Card = cardToBuy60 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 2, 1, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy60),
                    TokenSupply = TokenGroup.FromArray([0, 1, 2, 4, 2, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 2 purchasing #60, polymath needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 1, 2]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([2, 4, 3, 0, 3, 0]),
                },
                new PurchaseAction() { Card = cardToBuy72 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(cardToBuy72),
                    TokenSupply = TokenGroup.FromArray([2, 4, 3, 0, 4, 2]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - level 2 purchasing #72, multiple polymaths needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = [alreadyReserved35],
                    PlayerStore = TokenGroup.FromArray([2, 3, 1, 2, 2, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([4, 0, 1, 1, 1, 5]),
                },
                new PurchaseAction() { Card = alreadyReserved35 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 3, 1, 1, 2, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(alreadyReserved35),
                    TokenSupply = TokenGroup.FromArray([4, 0, 1, 2, 1, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - buying reserved #35");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = [alreadyReserved49],
                    PlayerStore = TokenGroup.FromArray([2, 1, 0, 0, 1, 1]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([4, 4, 1, 1, 1, 4]),
                },
                new PurchaseAction() { Card = alreadyReserved49 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([2, 0, 0, 0, 1, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(alreadyReserved49),
                    TokenSupply = TokenGroup.FromArray([4, 5, 1, 1, 1, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - buying reserved #49, polymath needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = [alreadyReserved49, alreadyReserved76],
                    PlayerStore = TokenGroup.FromArray([4, 0, 0, 2, 1, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([1, 3, 4, 0, 1, 5]),
                },
                new PurchaseAction() { Card = alreadyReserved76 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved49],
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 2, 1, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(alreadyReserved76),
                    TokenSupply = TokenGroup.FromArray([5, 3, 4, 0, 1, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - buying reserved #76 from two reserved cards");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = [alreadyReserved35, alreadyReserved49, alreadyReserved84],
                    PlayerStore = TokenGroup.FromArray([0, 0, 4, 3, 0, 2]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([4, 0, 0, 0, 1, 3]),
                },
                new PurchaseAction() { Card = alreadyReserved84 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved49, alreadyReserved35],
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(alreadyReserved84),
                    TokenSupply = TokenGroup.FromArray([4, 0, 4, 3, 1, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - buying reserved #84 from three reserved cards, multiple polymaths needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[0],
                    PlayerReservedCards = [alreadyReserved49, alreadyReserved84],
                    PlayerStore = TokenGroup.FromArray([0, 4, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList(),
                    TokenSupply = TokenGroup.FromArray([3, 3, 3, 3, 3, 5]),
                },
                new PurchaseAction() { Card = alreadyReserved49 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = [alreadyReserved84],
                    PlayerStore = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau33104.ToImmutableList().Add(alreadyReserved49),
                    TokenSupply = TokenGroup.FromArray([3, 5, 3, 3, 3, 5]),
                })
                .SetName("Purchase - Card Setup #11111 - Tableau #33104 - buying reserved #49 from two reserved cards");
        }

        private static IEnumerable<TestCaseData> LeaderEligibilityCases_Singular()
        {
            int testSeed = 22222;
            var randomizedCardSetup = GameFactory.GetRandomizedCardSetup(testSeed);

            // Level 1
            //+ [0] { DevelopmentCard { Id = 40, Name = Városfal, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 2, Green = 1, Brown = 1, Red = 0, Polymath = 0 }, Bonus = Red } }
            //+ [1] { DevelopmentCard { Id = 8, Name = Városháza, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 4, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Yellow } }
            //+ [2] { DevelopmentCard { Id = 16, Name = Templom, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 0, Green = 1, Brown = 1, Red = 1, Polymath = 0 }, Bonus = Blue } }
            //+ [3] { DevelopmentCard { Id = 29, Name = Kőfejtő, Level = 1, VictoryPoints = 0, Cost = TokenGroup { Yellow = 1, Blue = 1, Green = 1, Brown = 0, Red = 1, Polymath = 0 }, Bonus = Brown } }

            // Level 2
            //+ [0] { DevelopmentCard { Id = 55, Name = Csillagászat, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 4, Blue = 2, Green = 0, Brown = 0, Red = 1, Polymath = 0 }, Bonus = Green } }
            //+ [1] { DevelopmentCard { Id = 66, Name = Harci Szekerek, Level = 2, VictoryPoints = 1, Cost = TokenGroup { Yellow = 3, Blue = 2, Green = 2, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Red } }
            //+ [2] { DevelopmentCard { Id = 56, Name = Matematika, Level = 2, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 6, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [3] { DevelopmentCard { Id = 63, Name = Útrendszer, Level = 2, VictoryPoints = 2, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 0, Red = 5, Polymath = 0 }, Bonus = Brown } }

            // Level 3
            //+ [0] { DevelopmentCard { Id = 73, Name = Petra, Level = 3, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 2, Green = 3, Brown = 5, Red = 3, Polymath = 0 }, Bonus = Yellow } }
            //+ [1] { DevelopmentCard { Id = 90, Name = Olümpiai Zeusz Szobor, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 7, Red = 0, Polymath = 0 }, Bonus = Red } }
            //+ [2] { DevelopmentCard { Id = 74, Name = Rodoszi Kolosszus, Level = 3, VictoryPoints = 4, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 0, Red = 7, Polymath = 0 }, Bonus = Yellow } }
            //+ [3] { DevelopmentCard { Id = 82, Name = Uri Nagy Ziggurat, Level = 3, VictoryPoints = 3, Cost = TokenGroup { Yellow = 5, Blue = 3, Green = 0, Brown = 2, Red = 3, Polymath = 0 }, Bonus = Green } }

            var playerTableau42233 = new List<DevelopmentCard>();

            playerTableau42233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(4).ToList());
            playerTableau42233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(2).ToList());
            playerTableau42233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(2).ToList());
            playerTableau42233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Brown).Take(3).ToList());
            playerTableau42233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Red).Take(3).ToList());

            var cardToBuy8 = Cards.Level1.First(c => c.Id == 8);
            var cardToBuy16 = Cards.Level1.First(c => c.Id == 16);
            var cardToBuy55 = Cards.Level2.First(c => c.Id == 55);
            var cardToBuy80 = Cards.Level3.First(c => c.Id == 73);

            var alreadyReserved6 = Cards.Level1.First(c => c.Id == 6);
            var alreadyReserved48 = Cards.Level2.First(c => c.Id == 48);
            var alreadyReserved74 = Cards.Level3.First(c => c.Id == 74);

            var leaderJustinian = Leaders.ExtendedLeaders.First(l => l.Id == 19);
            var leaderPtolemy = Leaders.ExtendedLeaders.First(l => l.Id == 18);
            var leaderHanVuTi = Leaders.ExtendedLeaders.First(l => l.Id == 11);

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[2],
                    AvailableLeaders = [leaderHanVuTi, leaderJustinian, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new PurchaseAction() { Card = cardToBuy16 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderHanVuTi, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList().Add(cardToBuy16),
                    PlayerAcquiredLeaders = [leaderJustinian],
                })
                .SetName("Purchase - Leader Eligibility Singular - Card Setup #22222 - purchasing #16 - gets Justinian");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[2],
                    AvailableLeaders = [leaderHanVuTi, leaderJustinian, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new PurchaseAction() { Card = cardToBuy8 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderHanVuTi, leaderJustinian, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList().Add(cardToBuy8),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                })
                .SetName("Purchase - Leader Eligibility Singular - Card Setup #22222 - purchasing #8 - no leader");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[2],
                    AvailableLeaders = [leaderHanVuTi, leaderJustinian, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new PurchaseAction() { Card = cardToBuy55 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderHanVuTi, leaderJustinian],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList().Add(cardToBuy55),
                    PlayerAcquiredLeaders = [leaderPtolemy],
                })
                .SetName("Purchase - Leader Eligibility Singular - Card Setup #22222 - purchasing #55 - gets ptolemy");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[2],
                    AvailableLeaders = [leaderHanVuTi, leaderJustinian, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new PurchaseAction() { Card = alreadyReserved74 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderHanVuTi, leaderJustinian, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList().Add(alreadyReserved74),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                })
                .SetName("Purchase - Leader Eligibility Singular - Card Setup #22222 - purchasing #74 from hand - no leader");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    Level1 = randomizedCardSetup[0],
                    Level2 = randomizedCardSetup[1],
                    Level3 = randomizedCardSetup[2],
                    AvailableLeaders = [leaderHanVuTi, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList(),
                    PlayerAcquiredLeaders = [leaderJustinian],
                },
                new PurchaseAction() { Card = cardToBuy16 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderHanVuTi, leaderPtolemy],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerTableau = playerTableau42233.ToImmutableList().Add(cardToBuy16),
                    PlayerAcquiredLeaders = [leaderJustinian],
                })
                .SetName("Purchase - Leader Eligibility Singular - Card Setup #22222 - purchasing #16 - already has justinian - no leader");
        }

        private static IEnumerable<TestCaseData> LeaderEligibilityCases_Multiple()
        {
            var playerTableau43423 = new List<DevelopmentCard>();

            playerTableau43423.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(4).ToList());
            playerTableau43423.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(3).ToList());
            playerTableau43423.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(4).ToList());
            playerTableau43423.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Brown).Take(2).ToList());
            playerTableau43423.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Red).Take(3).ToList());

            var cardToBuy12 = Cards.Level1.First(c => c.Id == 12);
            var cardToBuy21 = Cards.Level1.First(c => c.Id == 21);
            var cardToBuy30 = Cards.Level1.First(c => c.Id == 30);

            var leaderDarius = Leaders.StandardLeaders.First(l => l.Id == 1);
            var leaderPericles = Leaders.StandardLeaders.First(l => l.Id == 2);

            var leaderJustinian = Leaders.ExtendedLeaders.First(l => l.Id == 19);
            var leaderQinShiHuang = Leaders.StandardLeaders.First(l => l.Id == 8);
            var leaderSolomon = Leaders.ExtendedLeaders.First(l => l.Id == 16);

            var leaderAlexander = Leaders.StandardLeaders.First(l => l.Id == 5);

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    AvailableLeaders = [leaderDarius, leaderAlexander],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]),
                    PlayerTableau = playerTableau43423.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 2, 2, 5]),
                },
                new PurchaseAction() { Card = cardToBuy12 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderAlexander],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau43423.ToImmutableList().Add(cardToBuy12),
                    PlayerAcquiredLeaders = [leaderDarius],
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 4, 2, 5]),
                })
                .SetName("Purchase - Leader Eligibility Multiple - Card Setup #22222 - purchasing #12 - no choice");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    AvailableLeaders = [leaderDarius, leaderPericles],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]),
                    PlayerTableau = playerTableau43423.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 2, 2, 5]),
                },
                new PurchaseAction() { Card = cardToBuy12 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    AvailableLeaders = [leaderDarius, leaderPericles],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau43423.ToImmutableList().Add(cardToBuy12),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 4, 2, 5]),
                })
                .SetName("Purchase - Leader Eligibility Multiple - Card Setup #22222 - purchasing #12 - Darius-Pericles choice");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderJustinian, leaderQinShiHuang, leaderSolomon],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau43423.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 2, 2, 5]),
                },
                new PurchaseAction() { Card = cardToBuy30 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [leaderJustinian, leaderQinShiHuang, leaderSolomon],
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = playerTableau43423.ToImmutableList().Add(cardToBuy30),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                    TokenSupply = TokenGroup.FromArray([2, 2, 2, 2, 2, 5]),
                })
                .SetName("Purchase - Leader Eligibility Multiple - Card Setup #22222 - purchasing #30 - Justinian-QinShiHuang-Solomon choice");

        }
    }
}
