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
        public void GetValidActions_Configurable_ShouldReturnTenDifferentThreeTokensActions(TokenGroup setup, DrawThreeTokensAction[] expectedActions)
        {
            // Arrange
            var players = new List<string> { "Player 1", "Player 2" };

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

        [Test]
        public void GetValidActions_InitialState_ShouldReturnFiveDifferentDoubleTokensActions()
        {
            // Arrange
            var players = new List<string> { "Player 1", "Player 2" };
            var gameState = GameFactory.GetNewGameState(GameConfiguration.Standard, players);

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            Assert.That(validActions.OfType<DrawDoubleTokensAction>().Count(), Is.EqualTo(5));
            Assert.That(validActions.OfType<DrawDoubleTokensAction>(), Is.Unique);
        }

        [Test]
        public void GetValidActions_ZeroTokensInPile_ShouldReturnFourDifferentThreeTokensActions()
        {
            // Arrange
            var players = new List<string> { "Player 1", "Player 2" };
            var gameState = GameFactory.GetNewGameState(GameConfiguration.Standard, players);

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            Assert.That(validActions.OfType<DrawThreeTokensAction>().Count(), Is.EqualTo(10));
            Assert.That(validActions.OfType<DrawThreeTokensAction>(), Is.Unique);
        }

        [Test]
        public void GetValidActions_LessThanFourTokensInPile_ShouldReturnFourDifferentDoubleTokensActions()
        {
            // Arrange
            var players = new List<string> { "Player 1", "Player 2" };
            var gameState = GameFactory.GetNewGameState(GameConfiguration.Standard, players);

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            Assert.That(validActions.OfType<DrawThreeTokensAction>().Count(), Is.EqualTo(10));
            Assert.That(validActions.OfType<DrawThreeTokensAction>(), Is.Unique);
        }
    }
}
