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
            // Layout sizes
            const int tokenColWidth = 10;
            const int deckColWidth = 22;
            const int marketColWidth = 18;
            const int marketCols = 4;
            const int levelBlocks = 3; // levels 3,2,1
            const int blockHeight = 5; // each level block height in text lines

            int totalHeight = levelBlocks * blockHeight;
            int totalWidth = tokenColWidth + 1 + deckColWidth + 1 + (marketCols * (marketColWidth + 1));

            // Helper: format token circle and count
            string FormatToken(string letter, int count, bool isPolymath = false)
            {
                // Represent token as (X)N or (O)N for polymath
                string circle = isPolymath ? "(O)" : $"({letter})";
                return $"{circle}{count}".PadRight(tokenColWidth);
            }

            // Gather token lines positioned evenly across totalHeight
            var tokens = new List<string>
            {
                FormatToken("S", gameState.TokenSupply.Yellow),  // S - Yellow
                FormatToken("K", gameState.TokenSupply.Blue),    // K - Blue
                FormatToken("Z", gameState.TokenSupply.Green),   // Z - Green
                FormatToken("B", gameState.TokenSupply.Brown),   // B - Brown
                FormatToken("P", gameState.TokenSupply.Red),     // P - Red
                FormatToken("O", gameState.TokenSupply.Polymath, isPolymath: true) // O - Polymath
            };

            string deckTitle(int level)
            {
                return level switch
                {
                    3 => $"Csodák - {gameState.Decks.Level3.Count}",
                    2 => $"Technológiák - {gameState.Decks.Level2.Count}",
                    1 => $"Épületek - {gameState.Decks.Level1.Count}",
                    _ => ""
                };
            }

            // Format a small card into blockHeight lines with given width
            string[] FormatCard(DevelopmentCard card, int width)
            {
                var lines = new string[blockHeight];

                string pad(string s) => s.Length > width - 2 ? s.Substring(0, width - 5) + "..." : s;

                string top = "+" + new string('-', width - 2) + "+";
                string bottom = "+" + new string('-', width - 2) + "+";

                string nameLine = $"| {pad(card?.Name ?? "Empty")}".PadRight(width - 1) + "|";
                string bonus = card != null ? card.Bonus.ToString().Substring(0, 1) : " ";
                string vp = card != null ? card.VictoryPoints.ToString() : " ";
                string mid = $"| {bonus} VP:{vp}".PadRight(width - 1) + "|";

                // cost summary short
                string costSummary = "";
                if (card?.Cost != null)
                {
                    var c = card.Cost;
                    costSummary = $"Req:Y{c.Yellow} K{c.Blue} Z{c.Green} B{c.Brown} P{c.Red}";
                }
                string costLine = $"| {pad(costSummary)}".PadRight(width - 1) + "|";

                lines[0] = top;
                lines[1] = nameLine;
                lines[2] = mid;
                lines[3] = costLine;
                lines[4] = bottom;

                return lines;
            }

            // Prepare empty card block
            string[] emptyCardBlock(int width)
            {
                var lines = new string[blockHeight];
                lines[0] = "+" + new string('-', width - 2) + "+";
                for (int i = 1; i < blockHeight - 1; i++)
                    lines[i] = "|" + new string(' ', width - 2) + "|";
                lines[blockHeight - 1] = "+" + new string('-', width - 2) + "+";
                return lines;
            }

            // Build a 2D array of strings for the entire visual output
            var canvas = new string[totalHeight];
            for (int i = 0; i < totalHeight; i++)
                canvas[i] = "";

            // Place tokens: distribute tokens evenly across totalHeight
            for (int t = 0; t < tokens.Count; t++)
            {
                int row = (int)Math.Round((double)t * (totalHeight - 1) / (tokens.Count - 1));
                for (int r = 0; r < totalHeight; r++)
                {
                    if (r == row)
                        canvas[r] += tokens[t].PadRight(tokenColWidth);
                    else
                        canvas[r] += new string(' ', tokenColWidth);
                }
            }

            // Separator after token column
            for (int r = 0; r < totalHeight; r++)
                canvas[r] += " ";

            // Deck column: each level block stacked (level3 top ... level1 bottom)
            for (int level = 3; level >= 1; level--)
            {
                int blockIndex = 3 - level; // 0 for level3
                int startRow = blockIndex * blockHeight;
                // Build deck box lines
                string top = "+" + new string('=', deckColWidth - 2) + "+";
                string bottom = "+" + new string('=', deckColWidth - 2) + "+";
                string title = $"| {deckTitle(level)}".PadRight(deckColWidth - 1) + "|";
                string spacer = "|" + new string(' ', deckColWidth - 2) + "|";

                for (int i = 0; i < blockHeight; i++)
                {
                    string line;
                    if (i == 0) line = top;
                    else if (i == 1) line = title;
                    else if (i == blockHeight - 1) line = bottom;
                    else line = spacer;

                    canvas[startRow + i] += line;
                }
            }

            // Separator after deck column
            for (int r = 0; r < totalHeight; r++)
                canvas[r] += " ";

            // Market columns: for each of 4 columns, render card blocks for levels 3..1 vertically aligned with deck blocks
            for (int col = 1; col <= marketCols; col++)
            {
                for (int level = 3; level >= 1; level--)
                {
                    int blockIndex = 3 - level;
                    int startRow = blockIndex * blockHeight;

                    DevelopmentCard card = null;
                    try
                    {
                        card = gameState.Market[level][col];
                    }
                    catch
                    {
                        card = null;
                    }

                    var block = card != null ? FormatCard(card, marketColWidth) : emptyCardBlock(marketColWidth);

                    for (int i = 0; i < blockHeight; i++)
                    {
                        canvas[startRow + i] += block[i];
                        // add separator between market columns
                    }
                }

                // after each market column add a single space separator
                for (int r = 0; r < totalHeight; r++)
                    canvas[r] += " ";
            }

            // Print built canvas
            Console.WriteLine();
            foreach (var line in canvas)
                Console.WriteLine(line);
            Console.WriteLine();
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

        public void DisplayInvalidCommand(GameAction command)
        {
            Console.WriteLine($"A megadott parancs ({command.GetType().Name}) nem érvényes a jelenlegi játékállapotban!");
        }

        public void DisplayGameTitle()
        {
            Console.WriteLine("   _____       _                _ _     _  __          __             _               ");
            Console.WriteLine("  / ____|     | |              | (_)   | | \\ \\        / /            | |              ");
            Console.WriteLine(" | (___  _ __ | | ___ _ __   __| |_  __| |  \\ \\  /\\  / /__  _ __   __| | ___ _ __ ___ ");
            Console.WriteLine("  \\___ \\| '_ \\| |/ _ \\ '_ \\ / _` | |/ _` |   \\ \\/  \\/ / _ \\| '_ \\ / _` |/ _ \\ '__/ __|");
            Console.WriteLine("  ____) | |_) | |  __/ | | | (_| | | (_| |    \\  /\\  / (_) | | | | (_| |  __/ |  \\__ \\");
            Console.WriteLine(" |_____/| .__/|_|\\___|_| |_|\\__,_|_|\\__,_|     \\/  \\/ \\___/|_| |_|\\__,_|\\___|_|  |___/");
            Console.WriteLine("        | |                                                                           ");
            Console.WriteLine("        |_|                                                                           ");
            Console.WriteLine("");
        }
    }
}
