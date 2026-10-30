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

        [TestCaseSource(nameof(DrawDoubleTokensActionCases))]
        public void Reduce_WithDifferentTokenSuppliesAndPlayerStores_ShouldReturnNewStateWithExpectedValues(TokenGroup tokenSupplySetup, TokenGroup player1StoreSetup, 
            DrawDoubleTokensAction actionToApply, 
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

        private static IEnumerable<TestCaseData> DrawDoubleTokensActionCases()
        {
            yield return new TestCaseData(
                TokenGroup.FromArray([4, 4, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0])},
                TokenGroup.FromArray([4, 2, 4, 4, 4, 5]),
                TokenGroup.FromArray([0, 2, 0, 0, 0, 0]),
                2)
                .SetName("Draw Double Tokens - zero player tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 3, 1, 4, 0, 5]),
                TokenGroup.FromArray([2, 0, 1, 0, 0, 0]),
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                TokenGroup.FromArray([2, 3, 1, 2, 0, 5]),
                TokenGroup.FromArray([2, 0, 1, 2, 0, 0]),
                2)
                .SetName("Draw Double Tokens - 3 player tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([4, 3, 1, 0, 0, 5]),
                TokenGroup.FromArray([2, 0, 1, 2, 0, 0]),
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([2, 0, 0, 0, 0, 0]) },
                TokenGroup.FromArray([2, 3, 1, 0, 0, 5]),
                TokenGroup.FromArray([4, 0, 1, 2, 0, 0]),
                2)
                .SetName("Draw Double Tokens - 5 player tokens");

            yield return new TestCaseData(
                TokenGroup.FromArray([2, 3, 0, 0, 5, 5]),
                TokenGroup.FromArray([1, 1, 2, 1, 3, 0]),
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 0, 2, 0]) },
                TokenGroup.FromArray([2, 3, 0, 0, 3, 5]),
                TokenGroup.FromArray([1, 1, 2, 1, 5, 0]),
                2)
                .SetName("Draw Double Tokens - 8 player tokens - does not exceed");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 4, 1, 2, 0, 5]),
                TokenGroup.FromArray([2, 4, 0, 2, 1, 0]),
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 2, 0, 0, 0, 0]) },
                TokenGroup.FromArray([0, 2, 1, 2, 0, 5]),
                TokenGroup.FromArray([2, 6, 0, 2, 1, 0]),
                1)
                .SetName("Draw Double Tokens - 9 player tokens - exceeds");

            yield return new TestCaseData(
                TokenGroup.FromArray([0, 0, 0, 7, 0, 5]),
                TokenGroup.FromArray([3, 1, 3, 2, 1, 0]),
                new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray([0, 0, 0, 2, 0, 0]) },
                TokenGroup.FromArray([0, 0, 0, 5, 0, 5]),
                TokenGroup.FromArray([3, 1, 3, 4, 1, 0]),
                1)
                .SetName("Draw Double Tokens - 10 player tokens - exceeds");
        }
    }
}
