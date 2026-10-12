using SplendidWonders.Core.Setup;
using SplendidWonders.Core.Model;

namespace SplendidWonders.Gameplay.Commands
{
    public class GameAction
    {
        public string Description { get; set; }
    }

    public class DrawTokensAction : GameAction
    {
        public TokenGroup Tokens { get; set; }
    }

    public class DrawThreeTokensAction : DrawTokensAction
    {
        public DrawThreeTokensAction() { Description = "Draw three tokens, each from a different type."; }

        override public bool Equals(object? obj)
        {
            if (obj is DrawThreeTokensAction other)
            {
                return Tokens.Equals(other.Tokens);
            }
            return base.Equals(obj);
        }
    }

    public class DrawDoubleTokensAction : DrawTokensAction
    {
        public DrawDoubleTokensAction() { Description = "Draw two tokens of the same type."; }

        public override bool Equals(object? obj)
        {
            if (obj is DrawDoubleTokensAction other)
            {
                return Tokens.Equals(other.Tokens);
            }

            return base.Equals(obj);
        }
    }

    public class PurchaseAction : GameAction
    {
        public DevelopmentCard Card { get; set; }

        public PurchaseAction() { Description = "Purchase a card from the board."; }

        public override bool Equals(object? obj)
        {
            if (obj is PurchaseAction other)
            {
                return Card.Equals(other.Card);
            }

            return base.Equals(obj);
        }
    }
    public class ReserveAction : GameAction
    {
    }

    public class ReserveFromBoardAction : ReserveAction
    {
        public DevelopmentCard Card { get; set; }
        public ReserveFromBoardAction() { Description = "Reserve a card from the board."; }

        public override bool Equals(object? obj)
        {
            if (obj is ReserveFromBoardAction other)
            {
                return Card.Equals(other.Card);
            }

            return base.Equals(obj);
        }
    }

    public class ReserveFromDeckAction : ReserveAction
    {
        public int Level { get; set; }

        public ReserveFromDeckAction() { Description = "Reserve a card from the deck."; }

        public override bool Equals(object? obj)
        {
            if (obj is ReserveFromDeckAction other)
            {
                return Level == other.Level;
            }

            return base.Equals(obj);
        }
    }
}