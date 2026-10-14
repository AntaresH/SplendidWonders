using SplendidWonders.Core.Model;
using SplendidWonders.CLI.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.CLI.View
{
    internal sealed class ConsoleGameStateRenderer
    {
        private const int TokenColumnWidth = 7;
        private const int DeckColumnWidth = 21;
        private const int MarketColumnWidth = 24;
        private const int MarketColumnCount = 4;
        private const int DeckLevelCount = 3;
        private const int DeckBlockHeight = 8;
        private const int TokenBlockHeight = 3;
        private const int PlayerPanelWidth = 36;

        public void Render(GameState gameState)
        {
            var playerPanels = BuildPlayerPanels(gameState);
            int deckTotalHeight = DeckLevelCount * DeckBlockHeight;
            var tokenBlocks = new[]
            {
                (Lines: BuildTokenBlock(gameState.TokenSupply.Yellow), Color: GetRgbColor(DevelopmentBonus.Yellow)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Blue), Color: GetRgbColor(DevelopmentBonus.Blue)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Green), Color: GetRgbColor(DevelopmentBonus.Green)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Brown), Color: GetRgbColor(DevelopmentBonus.Brown)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Red), Color: GetRgbColor(DevelopmentBonus.Red)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Polymath, isPolymath: true), Color: new RgbColor(255, 255, 255))
            };
            int tokenTotalHeight = tokenBlocks.Length * TokenBlockHeight;
            int playerPanelsHeight = 0;
            foreach (var playerPanel in playerPanels)
                playerPanelsHeight = Math.Max(playerPanelsHeight, playerPanel.Lines.Count);

            int totalHeight = Math.Max(Math.Max(deckTotalHeight, tokenTotalHeight), playerPanelsHeight);

            var canvas = new string[totalHeight];
            Array.Fill(canvas, string.Empty);
            var colorRanges = new List<ColorRange>[totalHeight];
            for (int row = 0; row < totalHeight; row++)
                colorRanges[row] = new List<ColorRange>();

            int tokenStartRow = Math.Max(0, (totalHeight - tokenTotalHeight) / 2);
            for (int tokenIndex = 0; tokenIndex < tokenBlocks.Length; tokenIndex++)
            {
                int startRow = tokenStartRow + tokenIndex * TokenBlockHeight;
                var block = tokenBlocks[tokenIndex];
                for (int row = 0; row < TokenBlockHeight; row++)
                    AppendCanvas(canvas, colorRanges, startRow + row, block.Lines[row].PadRight(TokenColumnWidth), block.Color);
            }

            for (int row = 0; row < totalHeight; row++)
                AppendCanvas(canvas, colorRanges, row, new string(' ', Math.Max(0, TokenColumnWidth - canvas[row].Length)) + " ");

            for (int level = DeckLevelCount; level >= 1; level--)
            {
                int startRow = (DeckLevelCount - level) * DeckBlockHeight;
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

                    AppendCanvas(canvas, colorRanges, startRow + row, line);
                }
            }

            for (int row = 0; row < totalHeight; row++)
                AppendCanvas(canvas, colorRanges, row, " ");

            for (int column = 1; column <= MarketColumnCount; column++)
            {
                for (int level = DeckLevelCount; level >= 1; level--)
                {
                    int startRow = (DeckLevelCount - level) * DeckBlockHeight;
                    var block = FormatCard(gameState.Market[level, column], MarketColumnWidth);
                    for (int row = 0; row < DeckBlockHeight; row++)
                        AppendCanvas(canvas, colorRanges, startRow + row, block.Lines[row], relativeRanges: block.ColorRanges[row]);
                }

                for (int row = 0; row < totalHeight; row++)
                    AppendCanvas(canvas, colorRanges, row, " ");
            }

            foreach (var playerPanel in playerPanels)
            {
                for (int row = 0; row < totalHeight; row++)
                {
                    string panelLine = row < playerPanel.Lines.Count
                        ? playerPanel.Lines[row]
                        : new string(' ', PlayerPanelWidth);
                    AppendCanvas(canvas, colorRanges, row, "   " + panelLine,
                        relativeRanges: row == playerPanel.StorageLine ? playerPanel.StorageColorRanges : null);
                }
            }

            Console.WriteLine();
            for (int row = 0; row < totalHeight; row++)
                WriteCanvasLine(canvas[row], colorRanges[row]);
            Console.WriteLine();
        }

        private static void AppendColoredNumber(StringBuilder text, List<ColorRange> colorRanges, int value, RgbColor color, string separator = " ")
        {
            if (text.Length > 0)
                text.Append(separator);

            string number = value.ToString();
            colorRanges.Add(new ColorRange(text.Length, number.Length, color));
            text.Append(number);
        }

        private static void AppendCanvas(string[] canvas, List<ColorRange>[] colorRanges, int row, string text, RgbColor? color = null, IReadOnlyList<ColorRange> relativeRanges = null)
        {
            int start = canvas[row].Length;
            canvas[row] += text;

            if (color is not null)
            {
                colorRanges[row].Add(new ColorRange(start, text.Length, color.Value));
            }
            else if (relativeRanges is not null)
            {
                foreach (var range in relativeRanges)
                    colorRanges[row].Add(new ColorRange(start + range.Start, range.Length, range.Color));
            }
        }

        private static void WriteCanvasLine(string line, List<ColorRange> ranges)
        {
            int position = 0;
            foreach (var range in ranges)
            {
                Console.Write(line[position..range.Start]);
                Console.Write($"\x1b[38;2;{range.Color.Red};{range.Color.Green};{range.Color.Blue}m{line.Substring(range.Start, range.Length)}\x1b[0m");
                position = range.Start + range.Length;
            }

            Console.WriteLine(line[position..]);
        }

        private readonly record struct RgbColor(byte Red, byte Green, byte Blue);
        private readonly record struct ColorRange(int Start, int Length, RgbColor Color);
        private sealed record PlayerPanel(List<string> Lines, int StorageLine, List<ColorRange> StorageColorRanges);
        private sealed record RenderedBlock(string[] Lines, List<ColorRange>[] ColorRanges);

        private static List<PlayerPanel> BuildPlayerPanels(GameState gameState)
        {
            var panels = new List<PlayerPanel>();
            for (int playerIndex = 0; gameState.Players is not null && playerIndex < gameState.Players.Count; playerIndex++)
            {
                var lines = new List<string>();
                var player = gameState.Players[playerIndex];
                if (player is null)
                {
                    AddPlayerBlock(lines, string.Empty, WrapText(Resources.EmptyTableau, PlayerPanelWidth - 4));
                    AddPlayerBlock(lines, Resources.PlayerStorageTitle, new List<string>
                    {
                        string.Empty,
                        string.Format(Resources.ReservedCardsCountFormat, 0)
                    });
                    AddTableauBlock(lines, new List<string> { string.Empty, Resources.EmptyTableau }, DeckBlockHeight * 3);
                    panels.Add(new PlayerPanel(lines, -1, new List<ColorRange>()));
                    continue;
                }

                string playerName = ReferenceEquals(player, gameState.CurrentPlayer)
                    ? $"*** {player.Name} ***"
                    : player.Name;
                AddPlayerBlock(lines, string.Empty, WrapText(playerName, PlayerPanelWidth - 4));

                var storage = player.Store;
                var storageText = new StringBuilder();
                var storageColorRanges = new List<ColorRange>();
                AppendColoredNumber(storageText, storageColorRanges, storage.Yellow, GetRgbColor(DevelopmentBonus.Yellow));
                AppendColoredNumber(storageText, storageColorRanges, storage.Blue, GetRgbColor(DevelopmentBonus.Blue));
                AppendColoredNumber(storageText, storageColorRanges, storage.Green, GetRgbColor(DevelopmentBonus.Green));
                AppendColoredNumber(storageText, storageColorRanges, storage.Brown, GetRgbColor(DevelopmentBonus.Brown));
                AppendColoredNumber(storageText, storageColorRanges, storage.Red, GetRgbColor(DevelopmentBonus.Red));
                AppendColoredNumber(storageText, storageColorRanges, storage.Polymath, new RgbColor(255, 255, 255), "  ");
                storageText.Append("   ").Append(string.Format(Resources.ReservedCardsCountFormat, player.ReservedCards?.Count ?? 0));
                var playerStorageLines = new List<string>
                {
                    string.Empty,
                    storageText.ToString()
                };

                int storageLine = lines.Count + 3;
                for (int rangeIndex = 0; rangeIndex < storageColorRanges.Count; rangeIndex++)
                {
                    var range = storageColorRanges[rangeIndex];
                    storageColorRanges[rangeIndex] = range with { Start = range.Start + 5 };
                }

                AddPlayerBlock(lines, Resources.PlayerStorageTitle, playerStorageLines);

                var tableauLines = new List<string>();
                if (player.Tableau is not null)
                {
                    foreach (var card in player.Tableau)
                    {
                        if (card is not null)
                            tableauLines.AddRange(WrapText("-  " + card.Name, PlayerPanelWidth - 4));
                    }
                }

                if (tableauLines.Count == 0)
                    tableauLines.Add(Resources.EmptyTableau);

                tableauLines.Insert(0, string.Empty);
                AddTableauBlock(lines, tableauLines, DeckBlockHeight * 3);
                panels.Add(new PlayerPanel(lines, storageLine, storageColorRanges));
            }

            return panels;
        }

        private static void AddPlayerBlock(List<string> lines, string title, List<string> contentLines)
        {
            string border = "+" + new string('-', PlayerPanelWidth - 2) + "+";
            lines.Add(border);
            if (!string.IsNullOrEmpty(title))
                lines.Add(FormatPlayerPanelLine(title));
            for (int i = 0; i < contentLines.Count; i++)
            {
                bool isReservedCardCount = title == Resources.PlayerStorageTitle && i == contentLines.Count - 1;
                lines.Add(FormatPlayerPanelLine(contentLines[i], isReservedCardCount));
            }
            lines.Add(border);
        }

        private static void AddTableauBlock(List<string> lines, List<string> contentLines, int minimumPanelHeight)
        {
            int minimumContentHeight = minimumPanelHeight - lines.Count - 3;
            while (contentLines.Count < minimumContentHeight)
                contentLines.Add(string.Empty);

            AddPlayerBlock(lines, Resources.PlayerTableauTitle, contentLines);
        }

        private static string FormatPlayerPanelLine(string content, bool extraSpaceBeforeRightBorder = false)
        {
            int contentWidth = PlayerPanelWidth - 4;
            string rightBorder = extraSpaceBeforeRightBorder ? "  |" : " |";
            return "| " + content.PadRight(contentWidth) + rightBorder;
        }

        private static List<string> WrapText(string text, int width, int maxLines = int.MaxValue)
        {
            var lines = new List<string>();
            var currentLine = new StringBuilder();

            foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (currentLine.Length > 0 && currentLine.Length + 1 + word.Length > width)
                {
                    lines.Add(currentLine.ToString());
                    if (lines.Count == maxLines)
                        return lines;
                    currentLine.Clear();
                }

                string remaining = word;
                while (remaining.Length > width)
                {
                    if (currentLine.Length > 0)
                    {
                        lines.Add(currentLine.ToString());
                        if (lines.Count == maxLines)
                            return lines;
                        currentLine.Clear();
                    }

                    lines.Add(remaining[..width]);
                    if (lines.Count == maxLines)
                        return lines;
                    remaining = remaining[width..];
                }

                if (currentLine.Length > 0)
                    currentLine.Append(' ');
                currentLine.Append(remaining);
            }

            if (currentLine.Length > 0 && lines.Count < maxLines)
                lines.Add(currentLine.ToString());

            if (lines.Count == 0)
                lines.Add(string.Empty);

            return lines;
        }

        private static string[] BuildTokenBlock(int count, bool isPolymath = false)
        {
            char horizontalBorder = isPolymath ? '=' : '-';
            char verticalBorder = isPolymath ? (char)0x2551 : '|';
            var lines = new string[TokenBlockHeight];
            int innerWidth = Math.Max(3, TokenColumnWidth - 4);

            lines[0] = "  " + new string(horizontalBorder, innerWidth) + "  ";
            string content = count.ToString();
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

        private static RenderedBlock FormatCard(DevelopmentCard card, int width)
        {
            var lines = new string[DeckBlockHeight];
            var colorRanges = new List<ColorRange>[DeckBlockHeight];
            for (int row = 0; row < DeckBlockHeight; row++)
                colorRanges[row] = new List<ColorRange>();

            string top = "+" + new string('-', width - 2) + "+";
            int contentWidth = Math.Max(8, width - 4);

            var nameLines = WrapText(card.Name ?? string.Empty, contentWidth, 3);

            while (nameLines.Count < 3)
                nameLines.Add(string.Empty);

            RgbColor bonusColor = GetRgbColor(card.Bonus);
            string victoryPoints = card.VictoryPoints > 0 ? "{" + card.VictoryPoints + "}" : string.Empty;
            var cost = card.Cost;
            var costText = new StringBuilder();
            var costColorRanges = new List<ColorRange>();
            if (cost.Yellow > 0) AppendColoredNumber(costText, costColorRanges, cost.Yellow, GetRgbColor(DevelopmentBonus.Yellow));
            if (cost.Blue > 0) AppendColoredNumber(costText, costColorRanges, cost.Blue, GetRgbColor(DevelopmentBonus.Blue));
            if (cost.Green > 0) AppendColoredNumber(costText, costColorRanges, cost.Green, GetRgbColor(DevelopmentBonus.Green));
            if (cost.Brown > 0) AppendColoredNumber(costText, costColorRanges, cost.Brown, GetRgbColor(DevelopmentBonus.Brown));
            if (cost.Red > 0) AppendColoredNumber(costText, costColorRanges, cost.Red, GetRgbColor(DevelopmentBonus.Red));
            string costSummary = costText.ToString();
            if (costSummary.Length > contentWidth)
                costSummary = Ellipsize(costSummary, contentWidth);
            var visibleCostRanges = new List<ColorRange>();
            foreach (var range in costColorRanges)
            {
                if (range.Start < costSummary.Length)
                    visibleCostRanges.Add(range with { Length = Math.Min(range.Length, costSummary.Length - range.Start) });
            }

            if (victoryPoints.Length > contentWidth) victoryPoints = victoryPoints.Substring(0, contentWidth);
            int betweenPadding = Math.Max(0, contentWidth - victoryPoints.Length);
            string firstLineContent = new string(' ', betweenPadding) + victoryPoints;

            lines[0] = top;
            lines[1] = "| " + firstLineContent.PadRight(contentWidth) + " |";
            lines[2] = "|" + new string(' ', width - 2) + "|";
            lines[3] = CenterCardLine(nameLines[0], contentWidth);
            lines[4] = CenterCardLine(nameLines[1], contentWidth);
            lines[5] = CenterCardLine(nameLines[2], contentWidth);
            lines[6] = "| " + costSummary.PadRight(contentWidth) + " |";
            lines[7] = top;

            colorRanges[0].Add(new ColorRange(0, width, bonusColor));
            AddCardSideBorderRanges(colorRanges, 1, width, bonusColor);
            if (victoryPoints.Length > 0)
                colorRanges[1].Add(new ColorRange(2 + betweenPadding, victoryPoints.Length, bonusColor));
            for (int row = 2; row <= 5; row++)
                AddCardSideBorderRanges(colorRanges, row, width, bonusColor);
            for (int row = 0; row < nameLines.Count; row++)
            {
                if (nameLines[row].Length > 0)
                {
                    int titleStart = 2 + (contentWidth - nameLines[row].Length) / 2;
                    colorRanges[row + 3].Add(new ColorRange(titleStart, nameLines[row].Length, bonusColor));
                }
            }
            AddCardSideBorderRanges(colorRanges, 6, width, bonusColor);
            foreach (var range in visibleCostRanges)
                colorRanges[6].Add(range with { Start = range.Start + 2 });
            colorRanges[7].Add(new ColorRange(0, width, bonusColor));
            foreach (var ranges in colorRanges)
                ranges.Sort((left, right) => left.Start.CompareTo(right.Start));

            return new RenderedBlock(lines, colorRanges);
        }

        private static void AddCardSideBorderRanges(List<ColorRange>[] colorRanges, int row, int width, RgbColor color)
        {
            colorRanges[row].Add(new ColorRange(0, 1, color));
            colorRanges[row].Add(new ColorRange(width - 1, 1, color));
        }

        private static string DeckLineCentered(string content, int width)
        {
            int contentWidth = width - 4;
            return (char)0x2551 + " " + CenterText(content, contentWidth) + " " + (char)0x2551;
        }

        private static string CenterCardLine(string content, int width)
        {
            return "| " + CenterText(content, width) + " |";
        }

        private static string CenterText(string content, int width)
        {
            string value = content ?? string.Empty;
            if (value.Length > width)
                value = Ellipsize(value, width);

            int leftPadding = (width - value.Length) / 2;
            return new string(' ', leftPadding) + value + new string(' ', width - value.Length - leftPadding);
        }

        private static RgbColor GetRgbColor(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => new RgbColor(208, 208, 0),
                DevelopmentBonus.Blue => new RgbColor(80, 145, 255),
                DevelopmentBonus.Green => new RgbColor(0, 220, 50),
                DevelopmentBonus.Brown => new RgbColor(145, 92, 42),
                DevelopmentBonus.Red => new RgbColor(255, 80, 80),
                _ => new RgbColor(192, 192, 192)
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
