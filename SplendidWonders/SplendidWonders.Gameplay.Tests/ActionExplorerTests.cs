using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System.Collections.Immutable;
using System.Numerics;

namespace SplendidWonders.Gameplay.Tests
{
    public partial class ActionExplorerTests
    {
        GameStateBuilder defaultBuilder = new GameStateBuilder();

        [SetUp]
        public void Setup()
        {
            var defaultPlayers = 
                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    },
                    new Player()
                    {
                        Name = "Test player 2",
                        ReservedCards = ImmutableList<DevelopmentCard>.Empty,
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    },
                };

            var defaultLeaders = GameFactory.GetRandomizedLeaders(true, defaultPlayers.Count);
            var defaultCards = GameFactory.GetRandomizedCardSetup();
            var defaultTokenSupply = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]);

            defaultBuilder = defaultBuilder
                .WithCurrentPlayer(defaultPlayers[0])
                .WithLeaders(defaultLeaders)
                .WithLevel1(defaultCards[0])
                .WithLevel2(defaultCards[1])
                .WithLevel3(defaultCards[2])
                .WithPlayers(defaultPlayers)
                .WithTokenSupply(defaultTokenSupply);
        }

        [TestCaseSource(nameof(DrawThreeTokensActionCases))]
        public void GetValidActions_WithDifferentTokenSupplies_ShouldReturnExpectedThreeTokenActions(TokenGroup tokenSupplySetup, DrawThreeTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithTokenSupply(tokenSupplySetup)
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<DrawThreeTokensAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(DrawDoubleTokensActionCases))]
        public void GetValidActions_WithDifferentTokenSupplies_ShouldReturnExpectedDoubleTokensActions(TokenGroup setup, DrawDoubleTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithTokenSupply(setup)
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<DrawDoubleTokensAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(ReturnTokensActionCases))]
        public void GetValidActions_WithDifferentPlayerSetup_ShouldReturnExpectedReturnTokensActions(List<Player> playerSetup, ReturnTokensAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<ReturnTokensAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(ReserveFromBoardActionCases))]
        public void GetValidActions_WithDifferentPlayerAndMarketSetup_ShouldReturnExpectedReserveFromBoardActions(List<Player> playerSetup, CardGroup[] cardSetup, ReserveFromBoardAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithCurrentPlayer(playerSetup[0])
                .WithLevel1(cardSetup[0])
                .WithLevel2(cardSetup[1])
                .WithLevel3(cardSetup[2])
                .WithPlayers(playerSetup)
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<ReserveFromBoardAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(ReserveFromDeckActionCases))]
        public void GetValidActions_WithDifferentPlayerAndDeckSetup_ShouldReturnExpectedReserveFromDeckActions(List<Player> playerSetup, CardGroup[] cardSetup, ReserveFromDeckAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithCurrentPlayer(playerSetup[0])
                .WithLevel1(cardSetup[0])
                .WithLevel2(cardSetup[1])
                .WithLevel3(cardSetup[2])
                .WithPlayers(playerSetup)
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<ReserveFromDeckAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(SelectLeaderActionCases))]
        public void GetValidActions_WithDifferentLeaders_ShouldReturnExpectedSelectLeaderActions(List<Player> playerSetup, ImmutableList<Leader> leaderSetup, SelectLeaderAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithLeaders(leaderSetup)
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<SelectLeaderAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(PurchaseActionEmptyTableauCases))]
        public void GetValidActions_WithDifferentStoreAndEmptyTableau_ShouldReturnExpectedPurchaseActions(List<Player> playerSetup, PurchaseAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<PurchaseAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(PurchaseActionTableau42011Cases))]
        public void GetValidActions_WithDifferentStoreAndTableau42011_ShouldReturnExpectedPurchaseActions(List<Player> playerSetup, PurchaseAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<PurchaseAction>(), Is.EquivalentTo(expectedActions));
        }

        [TestCaseSource(nameof(PurchaseActionTableau20233CardSeed12345Cases))]
        public void GetValidActions_WithDifferentStoreAndTableau20233_ShouldReturnExpectedPurchaseActions(List<Player> playerSetup, CardGroup[] cardSetup, PurchaseAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithLevel1(cardSetup[0])
                .WithLevel2(cardSetup[1])
                .WithLevel3(cardSetup[2])
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);
            Assert.That(validActions.OfType<PurchaseAction>(), Is.EquivalentTo(expectedActions));
        }
    }
}
