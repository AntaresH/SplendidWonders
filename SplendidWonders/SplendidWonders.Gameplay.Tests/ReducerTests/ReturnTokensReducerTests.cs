using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
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

        [TestCaseSource(nameof(ReturnTokensActionCases))]
        public void Reduce_WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupplySetup, TokenGroup player1StoreSetup, 
            ReturnTokensAction actionToApply, 
            TokenGroup expectedTokenSupply, TokenGroup expectedPlayerStore, int expectedCurrentPlayerId)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(1)
                .WithPlayerStore(1, player1StoreSetup)
                .WithTokenSupply(tokenSupplySetup)
                .Build();

            // Act
            var newState = _sut.Reduce(baseState, actionToApply);

            // Assert
            Assert.IsNotNull(newState);
            Assert.That(newState.TokenSupply, Is.EqualTo(expectedTokenSupply));
            Assert.That(newState.Players[0].Store, Is.EqualTo(expectedPlayerStore));
            Assert.That(newState.CurrentPlayer.Id, Is.EqualTo(expectedCurrentPlayerId));
        }

        private static IEnumerable<TestCaseData> ReturnTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 3, 1, 2, 2, 0]),
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 0, 0, 0]) },
                TokenGroup.FromArray([4, 4, 5, 4, 4, 5]),
                TokenGroup.FromArray([3, 3, 0, 2, 2, 0]),
                2)
                .SetName("Return Tokens - return green token from 11");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 3, 0, 1, 4, 5]),
                TokenGroup.FromArray([3, 3, 2, 0, 3, 0]),
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 1, 0]) },
                TokenGroup.FromArray([2, 3, 0, 1, 5, 5]),
                TokenGroup.FromArray([3, 3, 2, 0, 2, 0]),
                2)
                .SetName("Return Tokens - return red token from 11");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 3, 3, 3, 0, 0]),
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0])},
                TokenGroup.FromArray([4, 6, 4, 4, 4, 5]),
                TokenGroup.FromArray([3, 1, 3, 3, 0, 0]),
                2)
                .SetName("Return Tokens - return same tokens from 12");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 3, 1, 2, 5, 5]),
                TokenGroup.FromArray([1, 3, 2, 4, 2, 0]),
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 0, 0]) },
                TokenGroup.FromArray([3, 3, 1, 3, 5, 5]),
                TokenGroup.FromArray([0, 3, 2, 3, 2, 0]),
                2)
                .SetName("Return Tokens - return different tokens from 12");

            yield return new TestCaseData(
                TokenGroup.FromArray([3, 0, 4, 2, 1, 5]),
                TokenGroup.FromArray([4, 1, 2, 3, 2, 0]),
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([3, 0, 0, 0, 0, 0]) },
                TokenGroup.FromArray([6, 0, 4, 2, 1, 5]),
                TokenGroup.FromArray([2, 1, 2, 3, 2, 0]),
                2)
                .SetName("Return Tokens - return same tokens from 13");

            yield return new TestCaseData(
                TokenGroup.FromArray([3, 0, 4, 2, 1, 5]),
                TokenGroup.FromArray([2, 1, 3, 3, 4, 0]),
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 2, 0, 0]) },
                TokenGroup.FromArray([4, 0, 4, 4, 1, 5]),
                TokenGroup.FromArray([1, 1, 3, 1, 4, 0]),
                2)
                .SetName("Return Tokens - return two token types tokens from 13");

            yield return new TestCaseData(
                TokenGroup.FromArray([3, 0, 4, 2, 1, 5]),
                TokenGroup.FromArray([3, 3, 2, 3, 2, 0]),
                new ReturnTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                TokenGroup.FromArray([4, 0, 5, 3, 0, 5]),
                TokenGroup.FromArray([2, 3, 2, 2, 1, 0]),
                2)
                .SetName("Return Tokens - return different tokens from 13");
        }
    }
}
