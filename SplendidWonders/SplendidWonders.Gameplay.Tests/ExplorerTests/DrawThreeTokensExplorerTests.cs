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
    public class DrawThreeTokensExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IActionExplorer<DrawThreeTokensAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new DrawThreeTokensExplorer();
        }

        [TestCaseSource(nameof(DrawThreeTokensActionCases))]
        public void Explore_WithDifferentTokenSupplies_ShouldReturnExpectedThreeTokenActions(TokenGroup tokenSupplySetup, DrawThreeTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = _defaultBuilder
                .WithTokenSupply(tokenSupplySetup)
                .Build();

            // Act
            var validActions = _sut.Explore(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.That(validActions, Is.EquivalentTo(expectedActions));
        }

        private static IEnumerable<TestCaseData> DrawThreeTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 1, 0, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1, 0]) },
                })
                .SetName("Draw Three Tokens - All are available");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 4, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 4, 0, 4, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Green unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 4, 0, 0, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Green and brown unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 4, 0, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 1, 0, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow and red unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([4, 0, 0, 0, 4, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1, 0]) },
                })
                .SetName("Draw Three Tokens - Blue and green and brown unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 4, 4, 0, 0, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 0, 0]) },
                })
                .SetName("Draw Three Tokens - Yellow and brown and red unavailable");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 4, 0, 0]),
                new[]
                {
                    new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0, 0]) },
                })
                .SetName("Draw Three Tokens - Only brown is available");

            yield return new TestCaseData(TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new DrawThreeTokensAction[] { })
                .SetName("Draw Three Tokens - Nothing is available");
        }
    }
}
