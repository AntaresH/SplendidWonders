using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.Input
{
    public interface IGameInput
    {
        GameAction ReadCommand(GameState gameState, IEnumerable<GameAction> validActions);
    }
}
