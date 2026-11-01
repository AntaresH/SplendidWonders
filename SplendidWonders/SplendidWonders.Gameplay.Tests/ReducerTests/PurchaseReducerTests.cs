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
            Assert.That(newState.CurrentPlayer.Id, Is.EqualTo(expected.ActivePlayerId));
            Assert.That(newState.TokenSupply, Is.EqualTo(expected.TokenSupply));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Store, Is.EqualTo(expected.PlayerStore));
            Assert.That(newState.Players[input.ActivePlayerId - 1].ReservedCards, Is.EquivalentTo(expected.PlayerReservedCards));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Tableau, Is.EquivalentTo(expected.PlayerTableau));

            AssertCardReplacement(baseState, newState, actionToApply);
        }

        [TestCaseSource(nameof(PurchaseCardSeed67890Cases))]
        public void Reduce_CardSeed67890_WithDifferentPurchaseScenarios_ShouldReturnExpectedPurchaseResults(
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
            Assert.That(newState.CurrentPlayer.Id, Is.EqualTo(expected.ActivePlayerId));
            Assert.That(newState.TokenSupply, Is.EqualTo(expected.TokenSupply));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Store, Is.EqualTo(expected.PlayerStore));
            Assert.That(newState.Players[input.ActivePlayerId - 1].ReservedCards, Is.EquivalentTo(expected.PlayerReservedCards));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Tableau, Is.EquivalentTo(expected.PlayerTableau));

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
                    TokenSupply = TokenGroup.FromArray([5, 2, 2, 2, 3, 5]),
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
                    TokenSupply = TokenGroup.FromArray([4, 2, 4, 4, 1, 5]),
                },
                new PurchaseAction() { Card = alreadyReserved64 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved58],
                    PlayerStore = TokenGroup.FromArray([1, 0, 1, 0, 0, 0]),
                    PlayerTableau = [alreadyReserved64],
                    TokenSupply = TokenGroup.FromArray([4, 5, 1, 3, 4, 5]),
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
                    TokenSupply = TokenGroup.FromArray([4, 5, 1, 3, 3, 5]),
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
                    TokenSupply = TokenGroup.FromArray([4, 4, 2, 3, 3, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #58 from two reserved cards, multiple polymaths needed");
        }
        private static IEnumerable<TestCaseData> PurchaseCardSeed67890Cases()
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
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 3, 0, 0, 0, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([1, 2, 4, 2, 0, 5]),
                },
                new PurchaseAction() { Card = cardToBuy2 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = [cardToBuy2],
                    TokenSupply = TokenGroup.FromArray([1, 5, 4, 2, 0, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 1 purchasing #2");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
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
                    ActivePlayerId = 1,
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
                    ActivePlayerId = 1,
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
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 1, 1, 3, 0, 0]),
                    PlayerTableau = [cardToBuy3],
                    TokenSupply = TokenGroup.FromArray([5, 2, 2, 2, 3, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - level 1 purchasing #3, much more tokens");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
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
                    ActivePlayerId = 1,
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
                    ActivePlayerId = 1,
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
                    ActivePlayerId = 1,
                    PlayerReservedCards = ImmutableList<DevelopmentCard>.Empty,
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    PlayerTableau = [alreadyReserved58],
                    TokenSupply = TokenGroup.FromArray([6, 7, 1, 1, 2, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #58, polymath needed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved58, alreadyReserved64],
                    PlayerStore = TokenGroup.FromArray([1, 3, 1, 2, 3, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([4, 2, 4, 4, 1, 5]),
                },
                new PurchaseAction() { Card = alreadyReserved64 },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved58],
                    PlayerStore = TokenGroup.FromArray([1, 0, 1, 0, 0, 0]),
                    PlayerTableau = [alreadyReserved64],
                    TokenSupply = TokenGroup.FromArray([4, 5, 1, 3, 4, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #64 from two reserved cards");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    PlayerReservedCards = [alreadyReserved23, alreadyReserved58, alreadyReserved89],
                    PlayerStore = TokenGroup.FromArray([1, 2, 1, 3, 3, 0]),
                    PlayerTableau = ImmutableList<DevelopmentCard>.Empty,
                    TokenSupply = TokenGroup.FromArray([4, 4, 1, 1, 1, 4]),
                },
                new PurchaseAction() { Card = alreadyReserved23 },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    PlayerReservedCards = [alreadyReserved58, alreadyReserved89],
                    PlayerStore = TokenGroup.FromArray([1, 1, 1, 1, 1, 0]),
                    PlayerTableau = [alreadyReserved23],
                    TokenSupply = TokenGroup.FromArray([4, 5, 1, 3, 3, 5]),
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
                    TokenSupply = TokenGroup.FromArray([4, 4, 2, 3, 3, 5]),
                })
                .SetName("Purchase - Card Setup #0 - Empty Tableau - buying reserved #58 from two reserved cards, multiple polymaths needed");
        }
    }
}
