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
    public class DrawThreeTokensReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<DrawThreeTokensAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new TokenActionReducer();
        }

        [TestCaseSource(nameof(DrawThreeTokensCases))]
        public void Reduce_WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupplySetup, TokenGroup player1StoreSetup, 
            DrawThreeTokensAction actionToApply, 
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

        private static IEnumerable<TestCaseData> DrawThreeTokensCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 1, 0, 1, 0])},
                TokenGroup.FromArray([4, 3, 3, 4, 3, 5]),
                TokenGroup.FromArray([0, 1, 1, 0, 1, 0]),
                2)
                .SetName("Draw Three Tokens - zero player tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 3, 1, 4, 0, 5]),
                TokenGroup.FromArray([2, 0, 1, 0, 0, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 0, 1, 0, 0]) },
                TokenGroup.FromArray([1, 2, 1, 3, 0, 5]),
                TokenGroup.FromArray([3, 1, 1, 1, 0, 0]),
                2)
                .SetName("Draw Three Tokens - 3 player tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 3, 1, 0, 0, 5]),
                TokenGroup.FromArray([2, 0, 1, 2, 0, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 1, 1, 0, 0, 0]) },
                TokenGroup.FromArray([1, 2, 0, 0, 0, 5]),
                TokenGroup.FromArray([3, 1, 2, 2, 0, 0]),
                2)
                .SetName("Draw Three Tokens - 5 player tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 3, 1, 1, 1, 5]),
                TokenGroup.FromArray([0, 1, 2, 1, 3, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 1, 1, 1, 0]) },
                TokenGroup.FromArray([2, 3, 0, 0, 0, 5]),
                TokenGroup.FromArray([0, 1, 3, 2, 4, 0]),
                2)
                .SetName("Draw Three Tokens - 7 player tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 1, 3, 2, 1, 5]),
                TokenGroup.FromArray([2, 4, 0, 1, 1, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 1, 0]) },
                TokenGroup.FromArray([2, 0, 3, 1, 0, 5]),
                TokenGroup.FromArray([2, 5, 0, 2, 2, 0]),
                1)
                .SetName("Draw Three Tokens - 8 player tokens - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([1, 0, 0, 2, 1, 5]),
                TokenGroup.FromArray([0, 1, 4, 2, 3, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 1, 1, 0]) },
                TokenGroup.FromArray([0, 0, 0, 1, 0, 5]),
                TokenGroup.FromArray([1, 1, 4, 3, 4, 0]),
                1)
                .SetName("Draw Three Tokens - 10 player tokens - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([1, 0, 0, 0, 1, 5]),
                TokenGroup.FromArray([0, 1, 2, 2, 3, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([1, 0, 0, 0, 1, 0]) },
                TokenGroup.FromArray([0, 0, 0, 0, 0, 5]),
                TokenGroup.FromArray([1, 1, 2, 2, 4, 0]),
                2)
                .SetName("Draw Three Tokens - 2-token supply");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 0, 0, 1, 0, 5]),
                TokenGroup.FromArray([3, 1, 0, 2, 2, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0, 0]) },
                TokenGroup.FromArray([0, 0, 0, 0, 0, 5]),
                TokenGroup.FromArray([3, 1, 0, 3, 2, 0]),
                2)
                .SetName("Draw Three Tokens - 1-token supply");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 1, 0, 1, 0, 5]),
                TokenGroup.FromArray([1, 3, 0, 2, 2, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 1, 0, 1, 0, 0]) },
                TokenGroup.FromArray([0, 0, 0, 0, 0, 5]),
                TokenGroup.FromArray([1, 4, 0, 3, 2, 0]),
                2)
                .SetName("Draw Three Tokens - 8 player tokens + 2-token supply - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 0, 0, 1, 1, 5]),
                TokenGroup.FromArray([3, 3, 0, 2, 1, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 1, 0]) },
                TokenGroup.FromArray([0, 0, 0, 0, 0, 5]),
                TokenGroup.FromArray([3, 3, 0, 3, 2, 0]),
                1)
                .SetName("Draw Three Tokens - 9 player tokens + 2-token supply - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 0, 0, 1, 0, 5]),
                TokenGroup.FromArray([2, 4, 0, 2, 1, 0]),
                new DrawThreeTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 1, 0, 0]) },
                TokenGroup.FromArray([0, 0, 0, 0, 0, 5]),
                TokenGroup.FromArray([2, 4, 0, 3, 1, 0]),
                2)
                .SetName("Draw Three Tokens - 9 player tokens + 1-token supply - does not exceed");

        }
    }
}
