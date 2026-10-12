using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.Input
{
    public interface IGameInput
    {
        GameAction GetCommand(List<GameAction> validActions, GameState gameState);
    }
}
