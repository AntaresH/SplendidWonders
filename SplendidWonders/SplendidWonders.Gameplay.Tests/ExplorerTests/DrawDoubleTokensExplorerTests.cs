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
    public class DrawDoubleTokensExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IActionExplorer<DrawDoubleTokensAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new DrawDoubleTokensExplorer();
        }

        [TestCaseSource(nameof(DrawDoubleTokensActionCases))]
        public void Explore_WithDifferentTokenSupplies_ShouldReturnExpectedDoubleTokensActions(TokenGroup setup, DrawDoubleTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = _defaultBuilder
                .WithTokenSupply(setup)
                .Build();

            // Act
            var validActions = _sut.Explore(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.That(validActions, Is.EquivalentTo(expectedActions));
        }

        private static IEnumerable<TestCaseData> DrawDoubleTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 0]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) }
                })
                .SetName("Draw Double Tokens - All token types are available (>4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 4, 4, 4, 0]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) }
                })
                .SetName("Draw Double Tokens - Blue are unavailable (<4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 4, 2, 1, 0]),
                new[]
                {
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                    new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 2, 0, 0, 0]) }
                })
                .SetName("Draw Double Tokens - Blue, Brown and Red are unavailable (<4)");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 3, 0, 2, 1, 0]),
                new DrawDoubleTokensAction[] { })
                .SetName("Draw Double Tokens - No double draw is available (<4)");
        }
    }
}
