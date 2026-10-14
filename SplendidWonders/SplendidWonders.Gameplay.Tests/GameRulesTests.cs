using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System.Numerics;

namespace SplendidWonders.Gameplay.Tests
{
    public class GameRulesTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetValidActions_StandardTwoPlayerGame_ShouldReturnListOfValidActions()
        {
            // Arrange
            var players = new List<string> { "Player 1", "Player 2" };
            var gameState = GameFactory.GetNewGameState(GameConfiguration.Standard, players);

            // Act
            var validActions = GameRules.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
        }

        [Test]
        public void GetValidActions_StandardTwoPlayerGameInitialState_ShouldReturnTenDifferentThreeTokensActions()
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
        public void GetValidActions_StandardTwoPlayerGameInitialState_ShouldReturnFiveDifferentDoubleTokensActions()
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
    }
}
