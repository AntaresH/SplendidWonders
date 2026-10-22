using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
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
                        ReservedCards = new List<DevelopmentCard>(),
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    },
                    new Player()
                    {
                        Name = "Test player 2",
                        ReservedCards = new List<DevelopmentCard>(),
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0])
                    },
                };

            var defaultDecks = new CardGroup()
            {
                Level1 = new Deck<DevelopmentCard>(Cards.Level1.ToList()),
                Level2 = new Deck<DevelopmentCard>(Cards.Level2.ToList()),
                Level3 = new Deck<DevelopmentCard>(Cards.Level3.ToList())
            };

            var defaultMarket = new CardGroup()
            {
                Level1 = new Deck<DevelopmentCard>(defaultDecks.Level1.Draw(4)),
                Level2 = new Deck<DevelopmentCard>(defaultDecks.Level2.Draw(4)),
                Level3 = new Deck<DevelopmentCard>(defaultDecks.Level3.Draw(4))
            };

            var defaultTokenSupply = TokenGroup.FromArray([0, 0, 0, 0, 0]);

            defaultBuilder = defaultBuilder
                .WithDecks(defaultDecks)
                .WithMarket(defaultMarket)
                .WithPlayers(defaultPlayers)
                .WithTokenSupply(defaultTokenSupply)
                .WithCurrentPlayer(defaultPlayers[0]);
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
            
            foreach(var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
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

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
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

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }

        [TestCaseSource(nameof(ReserveFromBoardActionCases))]
        public void GetValidActions_WithDifferentPlayerAndMarketSetup_ShouldReturnExpectedReserveFromBoardActions(List<Player> playerSetup, CardGroup marketSetup, ReserveFromBoardAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithPlayers(playerSetup)
                .WithMarket(marketSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }

        [TestCaseSource(nameof(ReserveFromDeckActionCases))]
        public void GetValidActions_WithDifferentPlayerAndDeckSetup_ShouldReturnExpectedReserveFromDeckActions(List<Player> playerSetup, CardGroup decksSetup, ReserveFromDeckAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithPlayers(playerSetup)
                .WithDecks(decksSetup)
                .WithCurrentPlayer(playerSetup[0])
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

            // Assert
            Assert.IsNotNull(validActions);
            Assert.IsInstanceOf<List<GameAction>>(validActions);

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }

        [TestCaseSource(nameof(SelectLeaderActionCases))]
        public void GetValidActions_WithDifferentLeaders_ShouldReturnExpectedSelectLeaderActions(List<Player> playerSetup, List<Leader> leaderSetup, SelectLeaderAction[] expectedActions)
        {
            // Arrange
            var gameState = defaultBuilder
                .WithPlayers(playerSetup)
                .WithCurrentPlayer(playerSetup[0])
                .WithLeaders(leaderSetup)
                .Build();

            // Act
            var validActions = ActionExplorer.GetValidActions(gameState);

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
