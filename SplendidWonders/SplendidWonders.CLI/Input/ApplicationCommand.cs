using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.Input
{
    public abstract class ApplicationCommand
    {
        public string CommandText { get; set; }
    }

    public class GameCommand : ApplicationCommand
    {
        public GameAction Action { get; set; }
    }

    public class PresentationCommand : ApplicationCommand{ }

    public class DisplayReservedCardsCommand : PresentationCommand { }
}
