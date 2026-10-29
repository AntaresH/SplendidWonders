using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using System.Collections.Immutable;
using System.Numerics;

namespace SplendidWonders.Gameplay.Tests.ExplorerTests
{
    public partial class ActionExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private ActionExplorer _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            var drawThreeTokensExplorer = new DrawThreeTokensExplorer();
            var drawDoubleTokensExplorer = new DrawDoubleTokensExplorer();
            var returnTokensExplorer = new ReturnTokensExplorer();
            var reserveFromBoardExplorer = new ReserveFromBoardExplorer();
            var reserveFromDeckExplorer = new ReserveFromDeckExplorer();
            var purchaseExplorer = new PurchaseExplorer();
            var selectLeaderExplorer = new SelectLeaderExplorer();

            _sut = new ActionExplorer(drawThreeTokensExplorer,
                drawDoubleTokensExplorer,
                returnTokensExplorer,
                reserveFromBoardExplorer,
                reserveFromDeckExplorer,
                purchaseExplorer,
                selectLeaderExplorer);
        }
    }
}
