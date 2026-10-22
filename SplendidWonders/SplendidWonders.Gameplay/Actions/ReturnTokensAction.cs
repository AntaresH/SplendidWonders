namespace SplendidWonders.Gameplay.Actions
{
    public class ReturnTokensAction : TokensAction
    {
        override public bool Equals(object? obj)
        {
            if (obj is ReturnTokensAction other)
            {
                return Tokens == other.Tokens;
            }

            return base.Equals(obj);
        }
    }
}