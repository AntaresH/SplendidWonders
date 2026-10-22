using SplendidWonders.Core.Setup;
using SplendidWonders.Core.Model;

namespace SplendidWonders.Gameplay.Actions
{
    public abstract class GameAction
    {
    }

    public abstract class TokensAction : GameAction
    {
        public TokenGroup Tokens { get; set; }
    }

    public abstract class ReserveAction : GameAction
    {
    }
}