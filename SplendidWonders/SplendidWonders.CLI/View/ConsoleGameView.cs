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
            const int tokenColWidth = 8;   
            const int deckColWidth = 21;
            const int marketColWidth = 24;
            const int marketCols = 4;
            const int levelBlocks = 3; // levels 3,2,1
            const int deckBlockHeight = 7; // each deck level height
            const int tokenBlockHeight = 3; // ascii circle height per token (bigger circle)

            int deckTotalHeight = levelBlocks * deckBlockHeight;
            int tokensCount = 6; // S,K,Z,B,P,O
            int tokenTotalHeight = tokensCount * tokenBlockHeight;

            int totalHeight = Math.Max(deckTotalHeight, tokenTotalHeight);

            // Center decks vertically if tokens require more height
            int deckStartRow = (totalHeight - deckTotalHeight) / 2;

            // Helper: build token block (5 lines) with bigger ascii circle containing the letter+count centered
            string[] BuildTokenBlock(char letter, int count, bool isPolymath = false)
            {
                var horizBorderLetter = isPolymath ? '=' : '-';
                var vertBorderLetter = isPolymath ? (char)0x2551 : '|';

                var lines = new string[tokenBlockHeight];
                // Ensure tokenColWidth is at least 7
                int innerWidth = Math.Max(3, tokenColWidth - 4);

                // top dashes
                lines[0] = "  " + new string(horizBorderLetter, innerWidth) + "  ";
                // middle with letter+count centered
                string content = (isPolymath ? 'O'.ToString() : letter.ToString()) + count.ToString();
                if (content.Length > innerWidth)
                    content = content.Substring(0, innerWidth);
                int leftPad = (innerWidth - content.Length) / 2;
                int rightPad = innerWidth - content.Length - leftPad;
                lines[1] = " " + vertBorderLetter + new string(' ', leftPad) + content + new string(' ', rightPad) + vertBorderLetter;
                // bottom dashes
                lines[2] = "  " + new string(horizBorderLetter, innerWidth) + "  ";

                // pad each line to tokenColWidth
                for (int i = 0; i < tokenBlockHeight; i++)
                    lines[i] = lines[i].PadRight(tokenColWidth);

                return lines;
            }

            var tokenBlocks = new List<string[]> {
                BuildTokenBlock('S', gameState.TokenSupply.Yellow),
                BuildTokenBlock('K', gameState.TokenSupply.Blue),
                BuildTokenBlock('Z', gameState.TokenSupply.Green),
                BuildTokenBlock('B', gameState.TokenSupply.Brown),
                BuildTokenBlock('P', gameState.TokenSupply.Red),
                BuildTokenBlock('O', gameState.TokenSupply.Polymath, isPolymath: true)
            };

            string deckTitle(int level)
            {
                return level switch
                {
                    3 => $"Csodák",
                    2 => $"Technológiák",
                    1 => $"Épületek",
                    _ => ""
                };
            }

            // Format a small card into deckBlockHeight lines with given width
            string[] FormatCard(DevelopmentCard card, int width)
            {
                var lines = new string[deckBlockHeight];
                string ellipsize(string s, int max) => s.Length > max ? s.Substring(0, Math.Max(0, max - 3)) + "..." : s;

                string top = "+" + new string('-', width - 2) + "+";
                string bottom = "+" + new string('-', width - 2) + "+";

                int contentWidth = Math.Max(8, width - 4); // safe content width

                // Wrap name into up to 3 lines without breaking words
                string name = card?.Name ?? "Empty";
                var nameLines = new List<string>();
                if (string.IsNullOrWhiteSpace(name)) nameLines.Add("");
                else
                {
                    var words = name.Split(' ');
                    var sb = new StringBuilder();
                    foreach (var w in words)
                    {
                        if (sb.Length == 0)
                            sb.Append(w);
                        else if (sb.Length + 1 + w.Length <= contentWidth)
                            sb.Append(' ').Append(w);
                        else
                        {
                            nameLines.Add(sb.ToString());
                            sb.Clear();
                            sb.Append(w);
                            if (nameLines.Count == 3) break;
                        }
                    }
                    if (sb.Length > 0 && nameLines.Count < 3)
                        nameLines.Add(sb.ToString());
                }
                // ensure exactly 3 entries
                while (nameLines.Count < 3) nameLines.Add("");
                for (int i = 0; i < nameLines.Count; i++)
                    if (nameLines[i].Length > contentWidth) nameLines[i] = ellipsize(nameLines[i], contentWidth);

                string MakeLine(string content)
                {
                    var c = content ?? "";
                    if (c.Length > contentWidth) c = ellipsize(c, contentWidth);
                    return "| " + c.PadRight(contentWidth) + " |";
                }

                string MakeLineCentered(string content)
                {
                    var c = content ?? "";
                    if (c.Length > contentWidth) c = ellipsize(c, contentWidth);
                    int leftPad = (contentWidth - c.Length) / 2;
                    int rightPad = contentWidth - c.Length - leftPad;
                    return "| " + new string(' ', leftPad) + c + new string(' ', rightPad) + " |";
                }

                // map bonus to single-letter Hungarian codes: S,K,Z,B,P
                string BonusLetter(DevelopmentBonus b)
                {
                    return b switch
                    {
                        DevelopmentBonus.Yellow => "S",
                        DevelopmentBonus.Blue => "K",
                        DevelopmentBonus.Green => "Z",
                        DevelopmentBonus.Brown => "B",
                        DevelopmentBonus.Red => "P",
                        _ => " "
                    };
                }

                string bonus = card != null ? BonusLetter(card.Bonus) : " ";
                string vp = card != null && card.VictoryPoints > 0 ? "{" + card.VictoryPoints.ToString() + "}" : "";
                string midContent = ""; // no longer used as a left-aligned mid; VP and bonus are on the first content row

                string costSummary = "";
                if (card?.Cost != null)
                {
                    var c = card.Cost;
                    var parts = new List<string>();
                    if (c.Yellow > 0) parts.Add($"S{c.Yellow}");
                    if (c.Blue > 0) parts.Add($"K{c.Blue}");
                    if (c.Green > 0) parts.Add($"Z{c.Green}");
                    if (c.Brown > 0) parts.Add($"B{c.Brown}");
                    if (c.Red > 0) parts.Add($"P{c.Red}");
                    costSummary = parts.Count > 0 ? string.Join(" ", parts) : "";
                    if (costSummary.Length > contentWidth) costSummary = ellipsize(costSummary, contentWidth);
                }

                // First content row: bonus on left, VP in braces on right (or blank if 0)
                string leftPart = bonus ?? "";
                string rightPart = vp ?? "";
                if (leftPart.Length > contentWidth) leftPart = leftPart.Substring(0, contentWidth);
                if (rightPart.Length > contentWidth) rightPart = rightPart.Substring(0, contentWidth);
                int spaceBetween = Math.Max(0, contentWidth - leftPart.Length - rightPart.Length);
                string firstLineContent = leftPart + new string(' ', spaceBetween) + rightPart;

                lines[0] = top;
                lines[1] = "| " + firstLineContent.PadRight(contentWidth) + " |";
                lines[2] = MakeLineCentered(nameLines[0]);
                lines[3] = MakeLineCentered(nameLines[1]);
                lines[4] = MakeLineCentered(nameLines[2]);
                lines[5] = "| " + costSummary.PadRight(contentWidth) + " |";
                lines[6] = bottom;

                return lines;
            }

            string[] EmptyCardBlock(int width)
            {
                var lines = new string[deckBlockHeight];
                lines[0] = "+" + new string('-', width - 2) + "+";
                for (int i = 1; i < deckBlockHeight - 1; i++)
                    lines[i] = "|" + new string(' ', width - 2) + "|";
                lines[deckBlockHeight - 1] = "+" + new string('-', width - 2) + "+";
                return lines;
            }

            // Build canvas
            var canvas = new string[totalHeight];
            for (int i = 0; i < totalHeight; i++)
                canvas[i] = "";

            // Place token column vertically (leftmost), centered to align with deck blocks
            int tokenStartRow = (totalHeight - tokenTotalHeight) / 2;
            if (tokenStartRow < 0) tokenStartRow = 0;
            for (int t = 0; t < tokenBlocks.Count; t++)
            {
                int startRow = tokenStartRow + t * tokenBlockHeight;
                var block = tokenBlocks[t];
                for (int i = 0; i < tokenBlockHeight; i++)
                {
                    canvas[startRow + i] += block[i].PadRight(tokenColWidth);
                }
            }

            // Ensure token column width is reserved on every row, then add single separator
            for (int r = 0; r < totalHeight; r++)
                canvas[r] = canvas[r].PadRight(tokenColWidth) + " ";

            // Deck column placed starting at deckStartRow
            string DeckLineCentered(string content, int width)
            {
                int contentWidth = Math.Max(8, width - 4);
                string ellipsizeLocal(string s, int max) => s.Length > max ? s.Substring(0, Math.Max(0, max - 3)) + "..." : s;
                var c = content ?? "";
                if (c.Length > contentWidth) c = ellipsizeLocal(c, contentWidth);
                int leftPad = (contentWidth - c.Length) / 2;
                int rightPad = contentWidth - c.Length - leftPad;
                return "| " + new string(' ', leftPad) + c + new string(' ', rightPad) + " |";
            }
            for (int level = 3; level >= 1; level--)
            {
                int blockIndex = 3 - level; // 0 for level3
                int startRow = deckStartRow + blockIndex * deckBlockHeight;

                string top = "+" + new string('=', deckColWidth - 2) + "+";
                string bottom = "+" + new string('=', deckColWidth - 2) + "+";
                // title centered on line 2, remaining card count centered on line 3
                string spacer = "|" + new string(' ', deckColWidth - 2) + "|";

                for (int i = 0; i < deckBlockHeight; i++)
                {
                    string line;
                    if (i == 0) line = top;
                    else if (i == 2) line = DeckLineCentered(deckTitle(level), deckColWidth);
                    else if (i == 4) line = DeckLineCentered(gameState.Decks[level].Count.ToString(), deckColWidth);
                    else if (i == deckBlockHeight - 1) line = bottom;
                    else line = spacer;

                    canvas[startRow + i] += line;
                }
            }

            // separator
            for (int r = 0; r < totalHeight; r++)
                canvas[r] += " ";

            // Market columns: render each column; align their level blocks with decks
            for (int col = 1; col <= marketCols; col++)
            {
                for (int level = 3; level >= 1; level--)
                {
                    int blockIndex = 3 - level;
                    int startRow = deckStartRow + blockIndex * deckBlockHeight;

                    DevelopmentCard card = null;
                    try
                    {
                        card = gameState.Market[level, col];
                    }
                    catch
                    {
                        card = null;
                    }

                    var block = card != null ? FormatCard(card, marketColWidth) : EmptyCardBlock(marketColWidth);

                    for (int i = 0; i < deckBlockHeight; i++)
                    {
                        canvas[startRow + i] += block[i];
                    }
                }

                // separator between market columns
                for (int r = 0; r < totalHeight; r++)
                    canvas[r] += " ";
            }

            // Print canvas
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
