using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.Input
{
    public class GameCommand
    {
        public GameAction Action { get; set; }

        public string CommandText { get; set; }
    }
}
