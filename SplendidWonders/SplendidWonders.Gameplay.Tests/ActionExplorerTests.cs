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

            var defaultLeaders = Leaders.StandardLeaders.Concat(Leaders.ExtendedLeaders).ToImmutableList();

            var defaultLevel1 = new CardGroup()
            {
                Deck = Cards.Level1.Skip(4).ToImmutableList(),
                Market = Cards.Level1.Take(4).ToImmutableList()
            };

            var defaultLevel2 = new CardGroup()
            {
                Deck = Cards.Level2.Skip(4).ToImmutableList(),
                Market = Cards.Level2.Take(4).ToImmutableList()
            };

            var defaultLevel3 = new CardGroup()
            {
                Deck = Cards.Level3.Skip(4).ToImmutableList(),
                Market = Cards.Level3.Take(4).ToImmutableList()
            };

            var defaultTokenSupply = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]);

            defaultBuilder = defaultBuilder
                .WithCurrentPlayer(defaultPlayers[0])
                .WithLeaders(defaultLeaders)
                .WithLevel1(defaultLevel1)
                .WithLevel2(defaultLevel2)
                .WithLevel3(defaultLevel3)
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

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
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

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
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

            foreach (var expectedAction in expectedActions)
            {
                Assert.That(validActions.Contains(expectedAction));
            }
        }
    }
}
