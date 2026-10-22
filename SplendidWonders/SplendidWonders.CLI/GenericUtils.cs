using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI
{
    internal class GenericUtils
    {
        public static Dictionary<string, string> CommandShortHandMap = new Dictionary<string, string>
        {
            { nameof(DrawThreeTokensAction), "H3" },
            { nameof(DrawDoubleTokensAction), "H2" },
            { nameof(ReserveFromBoardAction), "FJ" },
            { nameof(ReserveFromDeckAction), "FP" },
            { nameof(PurchaseAction), "KV" }
        };
    }
}
