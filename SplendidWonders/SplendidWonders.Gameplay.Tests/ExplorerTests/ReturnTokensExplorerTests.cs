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
    public class ReturnTokensExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IActionExplorer<ReturnTokensAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new ReturnTokensExplorer();
        }

        [TestCaseSource(nameof(ReturnTokensActionCases))]
        public void Explore_WithDifferentPlayerSetup_ShouldReturnExpectedReturnTokensActions(List<Player> playerSetup, ReturnTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = _defaultBuilder
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = _sut.Explore(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.That(validActions, Is.EquivalentTo(expectedActions));
        }

        private static IEnumerable<TestCaseData> ReturnTokensActionCases()
        {
            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([2, 2, 2, 2, 3, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 1, 0]) }
                })
                .SetName("Return Tokens - 11 player tokens, balanced");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([4, 2, 5, 0, 0, 0]),
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0, 0]) }
                })
                .SetName("Return Tokens - 11 player tokens, no brown and red tokens");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([3, 3, 2, 2, 2, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 1, 0]) },
                })
                .SetName("Return Tokens - 12 player tokens, balanced");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([4, 0, 3, 2, 3, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 1, 0]) },
                })
                .SetName("Return Tokens - 12 player tokens, no blue tokens");

            yield return new TestCaseData(
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([3, 2, 3, 3, 2, 0])
                    }
                },
                new[]
                {
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([3, 0, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 3, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 3, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 1, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 2, 0, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 2, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 2, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 2, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 1, 0, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                    new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1, 0]) },
                })
                .SetName("Return Tokens - 13 player tokens, balanced");
        }
    }
}
