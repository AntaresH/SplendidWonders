using SplendidWonders.Common;
using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.CLI.View
{
    internal class ConsoleGameActionsRenderer
    {
        internal void Render(List<GameAction> validActions)
        {
            Console.WriteLine(Strings.ValidCommands);

            if (validActions.OfType<DrawThreeTokensAction>().Any())
                Console.WriteLine(string.Format("{0} - {1}", GenericUtils.CommandShortHandMap[nameof(DrawThreeTokensAction)], Strings.DrawThreeTokensActionDescription));

            if (validActions.OfType<DrawDoubleTokensAction>().Any())
                Console.WriteLine(string.Format("{0} - {1}", GenericUtils.CommandShortHandMap[nameof(DrawDoubleTokensAction)], Strings.DrawDoubleTokensActionDescription));

            if (validActions.OfType<ReturnTokensAction>().Any())
                Console.WriteLine(string.Format("{0} - {1}", GenericUtils.CommandShortHandMap[nameof(ReturnTokensAction)], Strings.ReturnTokensActionDescription));

            if (validActions.OfType<ReserveFromBoardAction>().Any())
                Console.WriteLine(string.Format("{0} - {1}", GenericUtils.CommandShortHandMap[nameof(ReserveFromBoardAction)], Strings.ReserveFromBoardActionDescription));

            if (validActions.OfType<ReserveFromDeckAction>().Any())
                Console.WriteLine(string.Format("{0} - {1}", GenericUtils.CommandShortHandMap[nameof(ReserveFromDeckAction)], Strings.ReserveFromDeckActionDescription));

            if (validActions.OfType<SelectLeaderAction>().Any())
                Console.WriteLine(string.Format("{0} - {1}", GenericUtils.CommandShortHandMap[nameof(SelectLeaderAction)], Strings.SelectLeaderActionDescriptionFormat));
        }
    }
}