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
    public class SelectLeaderReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<SelectLeaderAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new SelectLeaderReducer();
        }


        [TestCaseSource(nameof(LeaderSelectionCases))]
        public void Reduce_LeaderEligibilityCases_WithDifferentPurchaseScenarios_ShouldReturnExpectedPurchaseResults(
            ReducerTestData input, SelectLeaderAction actionToApply, ReducerTestData expected)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(input.ActivePlayerId)
                .WithLeaders(input.AvailableLeaders)
                .WithPlayerAcquiredLeaders(input.ActivePlayerId, input.PlayerAcquiredLeaders)
                .Build();

            // Act
            var newState = _sut.Reduce(baseState, actionToApply);

            // Assert
            Assert.IsNotNull(newState);
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(expected.ActivePlayerId));
            Assert.That(newState.Leaders, Is.EquivalentTo(expected.AvailableLeaders));
            Assert.That(newState.Players[input.ActivePlayerId - 1].AcquiredLeaders, Is.EquivalentTo(expected.PlayerAcquiredLeaders));
        }

        private static IEnumerable<TestCaseData> LeaderSelectionCases()
        {
            var playerTableau33233 = new List<DevelopmentCard>()
            {
                Cards.Level1.First(c => c.Id == 1),         // yellow no VP development card
                Cards.Level1.First(c => c.Id == 2),         // yellow no VP development card
                Cards.Level1.First(c => c.Id == 3),         // yellow no VP development card
                Cards.Level1.First(c => c.Id == 9),         // blue no VP development card
                Cards.Level1.First(c => c.Id == 10),        // blue no VP development card
                Cards.Level1.First(c => c.Id == 11),        // blue no VP development card
                Cards.Level1.First(c => c.Id == 17),        // green no VP development card
                Cards.Level1.First(c => c.Id == 18),        // green 1 VP development card
                Cards.Level1.First(c => c.Id == 25),        // brown no VP development card
                Cards.Level1.First(c => c.Id == 26),        // brown no VP development card
                Cards.Level1.First(c => c.Id == 27),        // brown no VP development card
                Cards.Level1.First(c => c.Id == 35),        // red no VP development card
                Cards.Level1.First(c => c.Id == 36),        // red no VP development card
                Cards.Level1.First(c => c.Id == 37),        // red no VP development card
            };

            var augustus = Leaders.StandardLeaders.First(l => l.Id == 6);          // Augustus (blue-yellow-green)
            var qinShi = Leaders.StandardLeaders.First(l => l.Id == 7);            // Qin shi huang ti (green-brown-red)
            var chandragupta = Leaders.ExtendedLeaders.First(l => l.Id == 20);     // Chandragupta (green-brown-red)
            var pericles = Leaders.StandardLeaders.First(l => l.Id == 2);          // Pericles (blue-green)

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    AvailableLeaders = [augustus, qinShi],
                    PlayerTableau = playerTableau33233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new SelectLeaderAction() { Leader = qinShi  },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [augustus],
                    PlayerAcquiredLeaders = [qinShi],
                })
                .SetName("Select Leader - Two-way choice - QinShi");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    AvailableLeaders = [augustus, qinShi, chandragupta],
                    PlayerTableau = playerTableau33233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new SelectLeaderAction() { Leader = augustus },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [qinShi, chandragupta],
                    PlayerAcquiredLeaders = [augustus],
                })
                .SetName("Select Leader - Three-way choice - Augustus");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    AvailableLeaders = [augustus, qinShi, chandragupta],
                    PlayerTableau = playerTableau33233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new SelectLeaderAction() { Leader = qinShi },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [augustus, chandragupta],
                    PlayerAcquiredLeaders = [qinShi],
                })
                .SetName("Select Leader - Three-way choice - QinShi");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    AvailableLeaders = [augustus, qinShi, chandragupta],
                    PlayerTableau = playerTableau33233.ToImmutableList(),
                    PlayerAcquiredLeaders = ImmutableList<Leader>.Empty,
                },
                new SelectLeaderAction() { Leader = chandragupta },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    AvailableLeaders = [augustus, qinShi],
                    PlayerAcquiredLeaders = [chandragupta],
                })
                .SetName("Select Leader - Three-way choice - Chandragupta");
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
                new PurchaseFromBoardAction() { Card = cardToBuy12 },
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
                new PurchaseFromBoardAction() { Card = cardToBuy12 },
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
                new PurchaseFromBoardAction() { Card = cardToBuy30 },
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
