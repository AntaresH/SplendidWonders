using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System.Numerics;

namespace SplendidWonders.Gameplay.Tests
{
    public partial class GameRulesTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCaseSource(nameof(DrawThreeTokensActionCases))]
        public void GetValidActions_WithDifferentTokenSupplies_ShouldReturnExpectedThreeTokenActions(TokenGroup setup, DrawThreeTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = new GameStateBuilder()
                .WithTokenSupply(setup)
                .Build();

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            
            foreach(var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }

        [TestCaseSource(nameof(DrawDoubleTokensActionCases))]
        public void GetValidActions_WithDifferentTokenSupplies_ShouldReturnExpectedDoubleTokensActions(TokenGroup setup, DrawDoubleTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = new GameStateBuilder()
                .WithTokenSupply(setup)
                .Build();

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }

        [TestCaseSource(nameof(ReturnTokensActionCases))]
        public void GetValidActions_WithDifferentTokenSupplies_ShouldReturnExpectedDoubleTokensActions(TokenGroup setup, ReturnTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = new GameStateBuilder()
                .WithTokenSupply(setup)
                .Build();

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }

        [TestCaseSource(nameof(ReserveFromBoardActionCases))]
        public void GetValidActions_WithDifferentPlayerAndMarketSetup_ShouldReturnExpectedReserveFromBoardActions(List<Player> playerSetup, CardGroup marketSetup, ReserveFromBoardAction[] expectedActions)
        {
            // Arrange
            var gameState = new GameStateBuilder()
                .WithPlayers(playerSetup)
                .WithMarket(marketSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }
    }
}
