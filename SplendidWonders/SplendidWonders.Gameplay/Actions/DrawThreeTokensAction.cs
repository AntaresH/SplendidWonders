using System.Runtime.Versioning;

namespace SplendidWonders.Gameplay.Actions
{
    public class DrawThreeTokensAction : TokensAction
    {
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