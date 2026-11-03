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
    public class ReturnTokensReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<ReturnTokensAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new TokenActionReducer();
        }

        [TestCaseSource(nameof(ReturnTokensCases))]
        public void Reduce_WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(
            ReducerTestData input, ReturnTokensAction actionToApply, ReducerTestData expected)
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
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(expected.ActivePlayerId));
        }

        private static IEnumerable<TestCaseData> ReturnTokensCases()
        {
            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 3, 1, 2, 2, 0])
                },
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([4, 4, 5, 4, 4, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 3, 0, 2, 2, 0])
                })
                .SetName("Return Tokens - return green token from 11");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([2, 3, 0, 1, 4, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 3, 2, 0, 3, 0])
                },
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 1, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([2, 3, 0, 1, 5, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 3, 2, 0, 2, 0])
                })
                .SetName("Return Tokens - return red token from 11");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 3, 3, 3, 0, 0])
                },
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([4, 6, 4, 4, 4, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 1, 3, 3, 0, 0])
                })
                .SetName("Return Tokens - return same tokens from 12");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([2, 3, 1, 2, 5, 5]),
                    PlayerStore = TokenGroup.FromArray([1, 3, 2, 4, 2, 0])
                },
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([3, 3, 1, 3, 5, 5]),
                    PlayerStore = TokenGroup.FromArray([0, 3, 2, 3, 2, 0])
                })
                .SetName("Return Tokens - return different tokens from 12");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([3, 0, 4, 2, 1, 5]),
                    PlayerStore = TokenGroup.FromArray([4, 1, 2, 3, 2, 0])
                },
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([3, 0, 0, 0, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([6, 0, 4, 2, 1, 5]),
                    PlayerStore = TokenGroup.FromArray([1, 1, 2, 3, 2, 0])
                })
                .SetName("Return Tokens - return same tokens from 13");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([3, 0, 4, 2, 1, 5]),
                    PlayerStore = TokenGroup.FromArray([2, 1, 3, 3, 4, 0])
                },
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 2, 0, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([4, 0, 4, 4, 1, 5]),
                    PlayerStore = TokenGroup.FromArray([1, 1, 3, 1, 4, 0])
                })
                .SetName("Return Tokens - return two token types from 13");

            yield return new TestCaseData(
                new ReducerTestData()
                {
                    ActivePlayerId = 1,
                    TokenSupply = TokenGroup.FromArray([3, 0, 4, 2, 1, 5]),
                    PlayerStore = TokenGroup.FromArray([3, 3, 2, 3, 2, 0])
                },
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                new ReducerTestData()
                {
                    ActivePlayerId = 2,
                    TokenSupply = TokenGroup.FromArray([4, 0, 4, 3, 2, 5]),
                    PlayerStore = TokenGroup.FromArray([2, 3, 2, 2, 1, 0])
                })
                .SetName("Return Tokens - return different tokens from 13");
        }
    }
}
