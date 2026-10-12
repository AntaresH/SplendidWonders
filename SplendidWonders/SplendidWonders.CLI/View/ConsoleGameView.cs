using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.View
{
    public class ConsoleGameView : IGameView
    {
        public void DisplayGameOver(Dictionary<string, int> value)
        {
            Console.WriteLine("Vége a játéknak!");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Végső pontszámok:");

            foreach (var item in value.OrderBy(x => x.Value).Reverse())
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        public void DisplayGameState(GameState gameState)
        {
            throw new NotImplementedException();
        }

        public void DisplayValidActions(List<GameAction> validActions)
        {
            Console.WriteLine("Valid commands:");

            foreach (var command in validActions)
            {
                string shorthand = GenericUtils.CommandShortHandMap.ContainsKey(command.GetType().Name) ? GenericUtils.CommandShortHandMap[command.GetType().Name] : string.Empty;
                Console.WriteLine($"{command.GetType().Name} ({shorthand})");
            }
        }

        public void DisplayTurnStart(Player player)
        {
            Console.WriteLine($"{player.Name} köre.");
            Console.WriteLine();
        }

        public void DisplayInvalidCommandMessage()
        {
            Console.WriteLine("A megadott parancs nem érvényes a jelenlegi játékállapotban!");
            Console.WriteLine();
        }

        public void DisplayCommandCategoryRequest(List<string> validActionCategories)
        {
            Console.WriteLine($"Kérlek adj meg egy végrehajtandó parancsot az alábbiak közül: {String.Join(", ", validActionCategories)}");
            Console.WriteLine();
        }

        public void DisplayThreeTokensArgumentRequest()
        {
            Console.WriteLine("Kérlek add meg, hogy milyen színű lapkákat szeretnél húzni (S - sárga, K - kék, Z - zöld, B - barna, P - piros). Például, \"S B P\"). Háromnál kevesebb színt is megadhatsz, ha nem tudnál úgysem a játéktérről eleget felhúzni:");
            Console.WriteLine();
        }

        public void DisplayDoubleTokensArgumentRequest()
        {
            Console.WriteLine("Kérlek add meg, hogy milyen színű lapkából szeretnél kettőt húzni (S - sárga, K - kék, Z - zöld, B - barna, P - piros). Például, \"K\"):");
            Console.WriteLine();
        }

        public void DisplayPurchaseArgumentRequest()
        {
            Console.WriteLine("Kérlek válaszd ki a kártyát, amit meg szeretnél vásárolni. Add meg a kártya szintjét (1-3) és az oszlopszámot (1-4). Például: \"1,2\" vagy \"3,3\":");
            Console.WriteLine();
        }

        public void DisplayReserveFromDeckArgumentRequest()
        {
            Console.WriteLine("Kérlek válaszd ki a paklit (1-3), aminek legfelső lapját le szeretnéd foglalni. Például: \"1\"");
            Console.WriteLine();
        }

        public void DisplayReserveFromBoardArgumentRequest()
        {
            Console.WriteLine("Kérlek válaszd ki a kártyát, amit le szeretnél foglalni. Add meg a kártya szintjét (1-3) és az oszlopszámot (1-4). Például: \"1,2\" vagy \"3,3\":");
            Console.WriteLine();
        }
    }
}
