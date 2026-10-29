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
    public class SelectLeaderExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IActionExplorer<SelectLeaderAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new SelectLeaderExplorer();
        }

        [TestCaseSource(nameof(SelectLeaderActionCases))]
        public void GetValidActions_WithDifferentLeaders_ShouldReturnExpectedSelectLeaderActions(List<Player> playerSetup, ImmutableList<Leader> leaderSetup, SelectLeaderAction[] expectedActions)
        {
            // Arrange
            var gameState = _defaultBuilder
                .WithLeaders(leaderSetup)
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = _sut.Explore(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.That(validActions, Is.EquivalentTo(expectedActions));
        }

        private static IEnumerable<TestCaseData> SelectLeaderActionCases()
        {
            var ybgLeader = Leaders.StandardLeaders.First(leader => leader.Id == 6);
            var gorLeader = Leaders.StandardLeaders.First(leader => leader.Id == 8);
            var ybrLeader = Leaders.StandardLeaders.First(leader => leader.Id == 10);
            var ygrLeader = Leaders.ExtendedLeaders.First(leader => leader.Id == 20);

            var ybLeader = Leaders.StandardLeaders.First(leader => leader.Id == 1);
            var bgLeader = Leaders.StandardLeaders.First(leader => leader.Id == 2);
            var ygLeader = Leaders.ExtendedLeaders.First(leader => leader.Id == 11);

            var twoWaySelectionLeaders333 = new List<Leader> { ybgLeader, gorLeader, ybrLeader }.ToImmutableList();
            var threeWaySelectionLeaders333 = new List<Leader> { ybgLeader, ybrLeader, ygrLeader }.ToImmutableList();

            var twoWaySelectionLeaders44 = new List<Leader> { ybLeader, ygLeader, gorLeader }.ToImmutableList();
            var threeWaySelectionLeaders44 = new List<Leader> { ybLeader, ygLeader, bgLeader, gorLeader, ygrLeader }.ToImmutableList();

            var playerTableau333 = new List<DevelopmentCard>();

            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(3).ToList());
            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(3).ToList());
            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(3).ToList());
            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Red).Take(3).ToList());

            var playerTableau44 = new List<DevelopmentCard>();

            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(4).ToList());
            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(4).ToList());
            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(4).ToList());
            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Brown).Take(4).ToList());

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau333.ToImmutableList()
                    }
                },
                twoWaySelectionLeaders333,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybgLeader },
                    new SelectLeaderAction() { Leader = ybrLeader }
                })
                .SetName("Select Leader - Two 3-3-3 leaders are eligible");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau333.ToImmutableList()
                    }
                },
                threeWaySelectionLeaders333,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybgLeader },
                    new SelectLeaderAction() { Leader = ybrLeader },
                    new SelectLeaderAction() { Leader = ygrLeader }
                })
                .SetName("Select Leader - Three 3-3-3 leaders are eligible");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau44.ToImmutableList()
                    }
                },
                twoWaySelectionLeaders44,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybLeader },
                    new SelectLeaderAction() { Leader = ygLeader }
                })
                .SetName("Select Leader - Two 4-4 leaders are eligible");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau44.ToImmutableList()
                    }
                },
                threeWaySelectionLeaders44,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybLeader },
                    new SelectLeaderAction() { Leader = ygLeader },
                    new SelectLeaderAction() { Leader = bgLeader }
                })
                .SetName("Select Leader - Three 4-4 leaders are eligible");
        }
    }
}
