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

    public class ReserveAction : GameAction
    {
    }
}