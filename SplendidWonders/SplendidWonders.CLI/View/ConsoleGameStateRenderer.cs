using SplendidWonders.Core.Model;
using SplendidWonders.CLI.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.View
{
    internal sealed class ConsoleGameStateRenderer
    {
        private const int TokenColumnWidth = 8;
        private const int DeckColumnWidth = 21;
        private const int MarketColumnWidth = 24;
        private const int MarketColumnCount = 4;
        private const int DeckBlockHeight = 8;
        private const int TokenBlockHeight = 3;
        private const int PlayerPanelWidth = 36;

        public void Render(GameState gameState)
        {
            var playerPanels = BuildPlayerPanels(gameState);
            int deckTotalHeight = 3 * DeckBlockHeight;
            int tokenTotalHeight = 6 * TokenBlockHeight;
            int playerPanelsHeight = 0;
            foreach (var playerPanel in playerPanels)
                playerPanelsHeight = Math.Max(playerPanelsHeight, playerPanel.Count);

            int totalHeight = Math.Max(Math.Max(deckTotalHeight, tokenTotalHeight), playerPanelsHeight);
            int deckStartRow = 0;

            var tokenBlocks = new List<string[]>
            {
                BuildTokenBlock(Resources.ColorYellow[0], gameState.TokenSupply.Yellow),
                BuildTokenBlock(Resources.ColorBlue[0], gameState.TokenSupply.Blue),
                BuildTokenBlock(Resources.ColorGreen[0], gameState.TokenSupply.Green),
                BuildTokenBlock(Resources.ColorBrown[0], gameState.TokenSupply.Brown),
                BuildTokenBlock(Resources.ColorRed[0], gameState.TokenSupply.Red),
                BuildTokenBlock((char)0x2736, gameState.TokenSupply.Polymath, isPolymath: true)
            };

            var canvas = new string[totalHeight];
            for (int row = 0; row < totalHeight; row++)
                canvas[row] = string.Empty;

            int tokenStartRow = Math.Max(0, (totalHeight - tokenTotalHeight) / 2);
            for (int tokenIndex = 0; tokenIndex < tokenBlocks.Count; tokenIndex++)
            {
                int startRow = tokenStartRow + tokenIndex * TokenBlockHeight;
                var block = tokenBlocks[tokenIndex];
                for (int row = 0; row < TokenBlockHeight; row++)
                    canvas[startRow + row] += block[row].PadRight(TokenColumnWidth);
            }

            for (int row = 0; row < totalHeight; row++)
                canvas[row] = canvas[row].PadRight(TokenColumnWidth) + " ";

            for (int level = 3; level >= 1; level--)
            {
                int blockIndex = 3 - level;
                int startRow = deckStartRow + blockIndex * DeckBlockHeight;
                string border = "+" + new string('=', DeckColumnWidth - 2) + "+";
                string spacer = (char)0x2551 + new string(' ', DeckColumnWidth - 2) + (char)0x2551;

                for (int row = 0; row < DeckBlockHeight; row++)
                {
                    string line = row switch
                    {
                        0 => border,
                        3 => DeckLineCentered(GetDeckTitle(level), DeckColumnWidth),
                        5 => DeckLineCentered(gameState.Decks[level].Count.ToString(), DeckColumnWidth),
                        DeckBlockHeight - 1 => border,
                        _ => spacer
                    };

                    canvas[startRow + row] += line;
                }
            }

            for (int row = 0; row < totalHeight; row++)
                canvas[row] += " ";

            for (int column = 1; column <= MarketColumnCount; column++)
            {
                for (int level = 3; level >= 1; level--)
                {
                    int blockIndex = 3 - level;
                    int startRow = deckStartRow + blockIndex * DeckBlockHeight;
                    DevelopmentCard? card;
                    try
                    {
                        card = gameState.Market[level, column];
                    }
                    catch
                    {
                        card = null;
                    }

                    var block = card is not null ? FormatCard(card, MarketColumnWidth) : EmptyCardBlock(MarketColumnWidth);
                    for (int row = 0; row < DeckBlockHeight; row++)
                        canvas[startRow + row] += block[row];
                }

                for (int row = 0; row < totalHeight; row++)
                    canvas[row] += " ";
            }

            foreach (var playerPanel in playerPanels)
            {
                for (int row = 0; row < totalHeight; row++)
                {
                    string panelLine = row < playerPanel.Count
                        ? playerPanel[row]
                        : new string(' ', PlayerPanelWidth);
                    canvas[row] += " " + panelLine;
                }
            }

            Console.WriteLine();
            foreach (var line in canvas)
                Console.WriteLine(line);
            Console.WriteLine();
        }

        private static List<List<string>> BuildPlayerPanels(GameState gameState)
        {
            var panels = new List<List<string>>();
            for (int playerIndex = 0; gameState.Players is not null && playerIndex < gameState.Players.Count; playerIndex++)
            {
                var lines = new List<string>();
                var player = gameState.Players[playerIndex];
                if (player is null)
                {
                    AddPlayerBlock(lines, string.Empty, WrapPlayerText(Resources.EmptyTableau, PlayerPanelWidth - 4));
                    AddPlayerBlock(lines, Resources.PlayerStorageTitle, WrapPlayerText(Resources.EmptyTableau, PlayerPanelWidth - 4));
                    AddTableauBlock(lines, new List<string> { Resources.EmptyTableau }, DeckBlockHeight * 3);
                    panels.Add(lines);
                    continue;
                }

                string playerName = ReferenceEquals(player, gameState.CurrentPlayer)
                    ? $"*** {player.Name} ***"
                    : player.Name;
                AddPlayerBlock(lines, string.Empty, WrapPlayerText(playerName, PlayerPanelWidth - 4));

                var storage = player.Store;
                string tokenStorage = string.Join(" ",
                    $"{Resources.ColorYellow}:{storage?.Yellow ?? 0}",
                    $"{Resources.ColorBlue}:{storage?.Blue ?? 0}",
                    $"{Resources.ColorGreen}:{storage?.Green ?? 0}",
                    $"{Resources.ColorBrown}:{storage?.Brown ?? 0}",
                    $"{Resources.ColorRed}:{storage?.Red ?? 0}",
                    $"{(char)0x2736}:{storage?.Polymath ?? 0}");
                AddPlayerBlock(lines, Resources.PlayerStorageTitle, WrapPlayerText(tokenStorage, PlayerPanelWidth - 4));

                var tableauLines = new List<string>();
                if (player.Tableau is not null)
                {
                    foreach (var card in player.Tableau)
                    {
                        if (card is not null)
                            tableauLines.AddRange(WrapPlayerText("- " + card.Name, PlayerPanelWidth - 4));
                    }
                }

                if (tableauLines.Count == 0)
                    tableauLines.Add(Resources.EmptyTableau);

                AddTableauBlock(lines, tableauLines, DeckBlockHeight * 3);
                panels.Add(lines);
            }

            return panels;
        }

        private static void AddPlayerBlock(List<string> lines, string title, List<string> contentLines)
        {
            string border = "+" + new string('-', PlayerPanelWidth - 2) + "+";
            lines.Add(border);
            if (!string.IsNullOrEmpty(title))
                lines.Add(FormatPlayerPanelLine(title));
            foreach (var contentLine in contentLines)
                lines.Add(FormatPlayerPanelLine(contentLine));
            lines.Add(border);
        }

        private static void AddTableauBlock(List<string> lines, List<string> contentLines, int minimumPanelHeight)
        {
            int minimumContentHeight = minimumPanelHeight - lines.Count - 3;
            while (contentLines.Count < minimumContentHeight)
                contentLines.Add(string.Empty);

            AddPlayerBlock(lines, Resources.PlayerTableauTitle, contentLines);
        }

        private static string FormatPlayerPanelLine(string content)
        {
            int contentWidth = PlayerPanelWidth - 4;
            return "| " + content.PadRight(contentWidth) + " |";
        }

        private static List<string> WrapPlayerText(string text, int width)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();

            foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (currentLine.Length > 0 && currentLine.Length + 1 + word.Length > width)
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                }

                string remaining = word;
                while (remaining.Length > width)
                {
                    if (currentLine.Length > 0)
                    {
                        lines.Add(currentLine.ToString());
                        currentLine.Clear();
                    }

                    lines.Add(remaining.Substring(0, width));
                    remaining = remaining.Substring(width);
                }

                if (currentLine.Length > 0)
                    currentLine.Append(' ');
                currentLine.Append(remaining);
            }

            if (currentLine.Length > 0)
                lines.Add(currentLine.ToString());

            if (lines.Count == 0)
                lines.Add(string.Empty);

            return lines;
        }

        private static string[] BuildTokenBlock(char letter, int count, bool isPolymath = false)
        {
            char horizontalBorder = isPolymath ? '=' : '-';
            char verticalBorder = isPolymath ? (char)0x2551 : '|';
            var lines = new string[TokenBlockHeight];
            int innerWidth = Math.Max(3, TokenColumnWidth - 4);

            lines[0] = "  " + new string(horizontalBorder, innerWidth) + "  ";
            string content = letter.ToString() + count;
            if (content.Length > innerWidth)
                content = content.Substring(0, innerWidth);

            int leftPadding = (innerWidth - content.Length) / 2;
            int rightPadding = innerWidth - content.Length - leftPadding;
            lines[1] = " " + verticalBorder + new string(' ', leftPadding) + content + new string(' ', rightPadding) + verticalBorder;
            lines[2] = "  " + new string(horizontalBorder, innerWidth) + "  ";

            for (int row = 0; row < TokenBlockHeight; row++)
                lines[row] = lines[row].PadRight(TokenColumnWidth);

            return lines;
        }

        private static string GetDeckTitle(int level)
        {
            return level switch
            {
                3 => Resources.Level3Title,
                2 => Resources.Level2Title,
                1 => Resources.Level1Title,
                _ => string.Empty
            };
        }

        private static string[] FormatCard(DevelopmentCard? card, int width)
        {
            var lines = new string[DeckBlockHeight];
            string top = "+" + new string('-', width - 2) + "+";
            int contentWidth = Math.Max(8, width - 4);

            string name = card?.Name ?? "Empty";
            var nameLines = new List<string>();
            if (string.IsNullOrWhiteSpace(name))
            {
                nameLines.Add(string.Empty);
            }
            else
            {
                var words = name.Split(' ');
                var builder = new StringBuilder();
                foreach (var word in words)
                {
                    if (builder.Length == 0)
                        builder.Append(word);
                    else if (builder.Length + 1 + word.Length <= contentWidth)
                        builder.Append(' ').Append(word);
                    else
                    {
                        nameLines.Add(builder.ToString());
                        builder.Clear();
                        builder.Append(word);
                        if (nameLines.Count == 3)
                            break;
                    }
                }

                if (builder.Length > 0 && nameLines.Count < 3)
                    nameLines.Add(builder.ToString());
            }

            while (nameLines.Count < 3)
                nameLines.Add(string.Empty);

            for (int index = 0; index < nameLines.Count; index++)
            {
                if (nameLines[index].Length > contentWidth)
                    nameLines[index] = Ellipsize(nameLines[index], contentWidth);
            }

            string bonus = card is not null ? GetBonusLetter(card.Bonus) : " ";
            string victoryPoints = card is not null && card.VictoryPoints > 0 ? "{" + card.VictoryPoints + "}" : string.Empty;
            string costSummary = string.Empty;
            if (card?.Cost is not null)
            {
                var cost = card.Cost;
                var parts = new List<string>();
                if (cost.Yellow > 0) parts.Add($"{Resources.ColorYellow}{cost.Yellow}");
                if (cost.Blue > 0) parts.Add($"{Resources.ColorBlue}{cost.Blue}");
                if (cost.Green > 0) parts.Add($"{Resources.ColorGreen}{cost.Green}");
                if (cost.Brown > 0) parts.Add($"{Resources.ColorBrown}{cost.Brown}");
                if (cost.Red > 0) parts.Add($"{Resources.ColorRed}{cost.Red}");
                costSummary = string.Join(" ", parts);
                if (costSummary.Length > contentWidth)
                    costSummary = Ellipsize(costSummary, contentWidth);
            }

            if (bonus.Length > contentWidth) bonus = bonus.Substring(0, contentWidth);
            if (victoryPoints.Length > contentWidth) victoryPoints = victoryPoints.Substring(0, contentWidth);
            int betweenPadding = Math.Max(0, contentWidth - bonus.Length - victoryPoints.Length);
            string firstLineContent = bonus + new string(' ', betweenPadding) + victoryPoints;

            lines[0] = top;
            lines[1] = "| " + firstLineContent.PadRight(contentWidth) + " |";
            lines[2] = "|" + new string(' ', width - 2) + "|";
            lines[3] = CenterCardLine(nameLines[0], contentWidth);
            lines[4] = CenterCardLine(nameLines[1], contentWidth);
            lines[5] = CenterCardLine(nameLines[2], contentWidth);
            lines[6] = "| " + costSummary.PadRight(contentWidth) + " |";
            lines[7] = top;
            return lines;
        }

        private static string[] EmptyCardBlock(int width)
        {
            var lines = new string[DeckBlockHeight];
            lines[0] = "+" + new string('-', width - 2) + "+";
            for (int row = 1; row < DeckBlockHeight - 1; row++)
                lines[row] = "|" + new string(' ', width - 2) + "|";
            lines[DeckBlockHeight - 1] = "+" + new string('-', width - 2) + "+";
            return lines;
        }

        private static string DeckLineCentered(string content, int width)
        {
            int contentWidth = width - 4;
            string value = content ?? string.Empty;
            if (value.Length > contentWidth)
                value = Ellipsize(value, contentWidth);

            int leftPadding = (contentWidth - value.Length) / 2;
            int rightPadding = contentWidth - value.Length - leftPadding;
            return (char)0x2551 + " " + new string(' ', leftPadding) + value + new string(' ', rightPadding) + " " + (char)0x2551;
        }

        private static string CenterCardLine(string content, int width)
        {
            string value = content ?? string.Empty;
            if (value.Length > width)
                value = Ellipsize(value, width);

            int leftPadding = (width - value.Length) / 2;
            int rightPadding = width - value.Length - leftPadding;
            return "| " + new string(' ', leftPadding) + value + new string(' ', rightPadding) + " |";
        }

        private static string GetBonusLetter(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => Resources.ColorYellow,
                DevelopmentBonus.Blue => Resources.ColorBlue,
                DevelopmentBonus.Green => Resources.ColorGreen,
                DevelopmentBonus.Brown => Resources.ColorBrown,
                DevelopmentBonus.Red => Resources.ColorRed,
                _ => " "
            };
        }

        private static string Ellipsize(string value, int maxLength)
        {
            return value.Length > maxLength
                ? value.Substring(0, Math.Max(0, maxLength - 3)) + "..."
                : value;
        }
    }
}
