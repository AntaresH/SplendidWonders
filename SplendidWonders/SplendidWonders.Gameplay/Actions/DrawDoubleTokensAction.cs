namespace SplendidWonders.Gameplay.Actions
{
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
}