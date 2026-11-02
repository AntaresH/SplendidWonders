using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using SplendidWonders.Gameplay.Tests.TestDataModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests.ReducerTests
{
    public class DrawDoubleTokensReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<DrawDoubleTokensAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new TokenActionReducer();
        }

        [TestCaseSource(nameof(DrawDoubleTokensCases))]
        public void Reduce_WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(
            ReducerTestData input,
            DrawDoubleTokensAction actionToApply,
            ReducerTestData expected)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(input.ActivePlayerId)
                .WithPlayerStore(input.ActivePlayerId, input.PlayerStore)
                .WithTokenSupply(input.TokenSupply)
                .Build();

            // Act
            var newState = _sut.Reduce(baseState, actionToApply);

            // Assert
            Assert.IsNotNull(newState);
            Assert.That(newState.TokenSupply, Is.EqualTo(expected.TokenSupply));
            Assert.That(newState.Players[input.ActivePlayerId - 1].Store, Is.EqualTo(expected.PlayerStore));
            Assert.That(newState.CurrentPlayer.Id, Is.EqualTo(expected.ActivePlayerId));
        }

        private static IEnumerable<TestCaseData> DrawDoubleTokensCases()
        {
            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                    PlayerStore = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                },
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0])},
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([4, 2, 4, 4, 4, 5]),
                    PlayerStore = TokenGroup.FromArray([0, 2, 0, 0, 0, 0])
                })
                .SetName("Draw Double Tokens - zero player tokens");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([2, 3, 1, 2, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([2, 0, 1, 0, 0, 0])
                },
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([2, 3, 1, 0, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([2, 0, 1, 2, 0, 0])
                })
                .SetName("Draw Double Tokens - 3 player tokens");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([4, 3, 1, 0, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([2, 0, 1, 2, 0, 0])
                },
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([2, 3, 1, 0, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([4, 0, 1, 2, 0, 0])
                })
                .SetName("Draw Double Tokens - 5 player tokens");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([2, 3, 0, 0, 5, 5]),
                    PlayerStore = TokenGroup.FromArray([1, 1, 2, 1, 3, 0])
                },
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([2, 3, 0, 0, 3, 5]),
                    PlayerStore = TokenGroup.FromArray([1, 1, 2, 1, 5, 0])
                })
                .SetName("Draw Double Tokens - 8 player tokens - does not exceed");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([0, 4, 1, 2, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([2, 4, 0, 2, 1, 0])
                },
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([0, 2, 1, 2, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([2, 6, 0, 2, 1, 0])
                })
                .SetName("Draw Double Tokens - 9 player tokens - exceeds");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([0, 0, 0, 7, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 1, 3, 2, 1, 0])
                },  
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([0, 0, 0, 5, 0, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 1, 3, 4, 1, 0])
                })
                .SetName("Draw Double Tokens - 10 player tokens - exceeds");
        }
    }
}
