namespace SplendidWonders.Gameplay.Actions
{
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
}