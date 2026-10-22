using SplendidWonders.Core.Setup;
using SplendidWonders.Core.Model;

namespace SplendidWonders.Gameplay.Actions
{
    public class GameAction
    {
        public string Description { get; set; }
    }

    public class TokensAction : GameAction
    {
        public TokenGroup Tokens { get; set; }
    }

    public class ReturnTokensAction : TokensAction
    {
        public ReturnTokensAction() { Description = "Tegyél vissza fejlődéslapkákat a játéktérre!"; }

        override public bool Equals(object? obj)
        {
            if (obj is DrawThreeTokensAction other)
            {
                return Tokens == other.Tokens;
            }

            return base.Equals(obj);
        }

    }

    public class DrawThreeTokensAction : TokensAction
    {
        public DrawThreeTokensAction() { Description = "Húzz 1-1 fejlődéslapkát három különböző kategóriákból!"; }

        override public bool Equals(object? obj)
        {
            if (obj is DrawThreeTokensAction other)
            {
                return Tokens == other.Tokens;
            }

            return base.Equals(obj);
        }
    }

    public class DrawDoubleTokensAction : TokensAction
    {
        public DrawDoubleTokensAction() { Description = "Húzz 2 azonos kategóriájú fejlődéslapkát!"; }

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

        public PurchaseAction() { Description = "Vásárolj meg egy kártyát a játéktérről vagy a korábban általad foglaltak közül!"; }

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
        public ReserveFromBoardAction() { Description = "Fogljalj le 1 kártyát a játéktérről!"; }

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

        public ReserveFromDeckAction() { Description = "Foglald le egy általad kiválasztott pakli legfelső kártyáját!"; }

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