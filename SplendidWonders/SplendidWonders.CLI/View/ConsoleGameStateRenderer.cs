using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using SplendidWonders.Common;

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
        private const int LeaderBlockWidth = 20;
        private const int LeaderBlockHeight = 6;
        private const int LeaderBlockContentWidth = LeaderBlockWidth - 4;
        private const int PlayerPanelWidth = 36;
        private const int PlayerPanelContentWidth = PlayerPanelWidth - 4;
        private const int PlayerPanelSideWidth = 2;
        private const int PlayerTableauMinimumPanelHeight = DeckBlockHeight * 3;

        public void Render(GameState gameState)
        {
            var playerPanels = BuildPlayerPanels(gameState);
            var leaderBlocks = new List<RenderedBlock>();
            foreach (var leader in gameState.Leaders)
                leaderBlocks.Add(FormatLeader(leader));

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
                        5 => DeckLineCentered(gameState[level].Remaining.ToString(), DeckColumnWidth),
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
                    var block = FormatCard(gameState[level][column], MarketColumnWidth);
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
                    if (playerPanel.TableauColorRanges.TryGetValue(row, out var tableauColorRanges))
                    {
                        AppendCanvas(canvas, colorRanges, row, "   " + panelLine[..PlayerPanelSideWidth]);
                        AppendCanvas(canvas, colorRanges, row, panelLine.Substring(PlayerPanelSideWidth, PlayerPanelContentWidth), relativeRanges: tableauColorRanges);
                        AppendCanvas(canvas, colorRanges, row, panelLine[(PlayerPanelSideWidth + PlayerPanelContentWidth)..]);
                    }
                    else
                    {
                        IReadOnlyList<ColorRange> panelColorRanges = row == playerPanel.StorageLine
                            ? playerPanel.StorageColorRanges
                            : null;
                        AppendCanvas(canvas, colorRanges, row, "   " + panelLine,
                            relativeRanges: panelColorRanges);
                    }
                }
            }

            Console.WriteLine();
            if (leaderBlocks.Count > 0)
            {
                WriteLeaderRow(leaderBlocks, canvas);
                Console.WriteLine();
            }

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

        private static void AppendBonusNumbers(StringBuilder text, List<ColorRange> colorRanges, TokenGroup bonuses)
        {
            if (bonuses.Yellow > 0) AppendColoredNumber(text, colorRanges, bonuses.Yellow, GetRgbColor(DevelopmentBonus.Yellow));
            if (bonuses.Blue > 0) AppendColoredNumber(text, colorRanges, bonuses.Blue, GetRgbColor(DevelopmentBonus.Blue));
            if (bonuses.Green > 0) AppendColoredNumber(text, colorRanges, bonuses.Green, GetRgbColor(DevelopmentBonus.Green));
            if (bonuses.Brown > 0) AppendColoredNumber(text, colorRanges, bonuses.Brown, GetRgbColor(DevelopmentBonus.Brown));
            if (bonuses.Red > 0) AppendColoredNumber(text, colorRanges, bonuses.Red, GetRgbColor(DevelopmentBonus.Red));
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
            ranges.Sort((left, right) => left.Start.CompareTo(right.Start));
            int position = 0;
            foreach (var range in ranges)
            {
                Console.Write(line[position..range.Start]);
                Console.Write($"\x1b[38;2;{range.Color.Red};{range.Color.Green};{range.Color.Blue}m{line.Substring(range.Start, range.Length)}\x1b[0m");
                position = range.Start + range.Length;
            }

            Console.WriteLine(line[position..]);
        }

        private static void WriteLeaderRow(List<RenderedBlock> leaders, string[] board)
        {
            int boardWidth = 0;
            foreach (string line in board)
                boardWidth = Math.Max(boardWidth, line.Length);

            int rowWidth = Math.Max(boardWidth, leaders.Count * LeaderBlockWidth);
            int availableSpacing = rowWidth - leaders.Count * LeaderBlockWidth;
            for (int row = 0; row < LeaderBlockHeight; row++)
            {
                var line = new StringBuilder(rowWidth);
                var colorRanges = new List<ColorRange>();
                for (int leaderIndex = 0; leaderIndex < leaders.Count; leaderIndex++)
                {
                    int start = leaderIndex * LeaderBlockWidth
                        + (leaderIndex + 1) * availableSpacing / (leaders.Count + 1);
                    line.Append(' ', start - line.Length);
                    foreach (var range in leaders[leaderIndex].ColorRanges[row])
                        colorRanges.Add(range with { Start = start + range.Start });
                    line.Append(leaders[leaderIndex].Lines[row]);
                }

                line.Append(' ', rowWidth - line.Length);
                WriteCanvasLine(line.ToString(), colorRanges);
            }
        }

        private readonly record struct RgbColor(byte Red, byte Green, byte Blue);
        private readonly record struct ColorRange(int Start, int Length, RgbColor Color);
        private sealed record PlayerPanel(List<string> Lines, int StorageLine, List<ColorRange> StorageColorRanges, Dictionary<int, List<ColorRange>> TableauColorRanges);
        private sealed record RenderedBlock(string[] Lines, List<ColorRange>[] ColorRanges);
        private sealed record ColoredLine(string Text, List<ColorRange> ColorRanges);

        private static List<PlayerPanel> BuildPlayerPanels(GameState gameState)
        {
            var panels = new List<PlayerPanel>();
            for (int playerIndex = 0; gameState.Players is not null && playerIndex < gameState.Players.Count; playerIndex++)
            {
                var lines = new List<string>();
                var player = gameState.Players[playerIndex];
                if (player is null)
                {
                    AddPlayerBlock(lines, string.Empty, WrapText(Strings.EmptyTableau, PlayerPanelContentWidth));
                    AddPlayerBlock(lines, Strings.PlayerStorageTitle, new List<string>
                    {
                        string.Empty,
                        string.Format(Strings.ReservedCardsCountFormat, 0)
                    });
                    AddTableauBlock(lines, new List<string> { string.Empty, Strings.EmptyTableau }, PlayerTableauMinimumPanelHeight);
                    panels.Add(new PlayerPanel(lines, -1, new List<ColorRange>(), new Dictionary<int, List<ColorRange>>()));
                    continue;
                }

                string playerName = ReferenceEquals(player, gameState.ActivePlayer)
                    ? $"*** {player.Name} ***"
                    : player.Name;
                AddPlayerBlock(lines, string.Empty, WrapText(playerName, PlayerPanelContentWidth));

                var storage = player.Store;
                var storageText = new StringBuilder();
                var storageColorRanges = new List<ColorRange>();
                AppendColoredNumber(storageText, storageColorRanges, storage.Yellow, GetRgbColor(DevelopmentBonus.Yellow));
                AppendColoredNumber(storageText, storageColorRanges, storage.Blue, GetRgbColor(DevelopmentBonus.Blue));
                AppendColoredNumber(storageText, storageColorRanges, storage.Green, GetRgbColor(DevelopmentBonus.Green));
                AppendColoredNumber(storageText, storageColorRanges, storage.Brown, GetRgbColor(DevelopmentBonus.Brown));
                AppendColoredNumber(storageText, storageColorRanges, storage.Red, GetRgbColor(DevelopmentBonus.Red));
                AppendColoredNumber(storageText, storageColorRanges, storage.Polymath, new RgbColor(255, 255, 255), "  ");
                storageText.Append("   ").Append(string.Format(Strings.ReservedCardsCountFormat, player.ReservedCards?.Count ?? 0));
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

                AddPlayerBlock(lines, Strings.PlayerStorageTitle, playerStorageLines);

                var tableauLines = new List<string>();
                var tableauLineColorRanges = new List<List<ColorRange>>();
                if (player.Tableau is not null)
                {
                    foreach (var card in player.Tableau)
                    {
                        if (card is not null)
                        {
                            var cardLines = FormatTableauCard(card, PlayerPanelContentWidth);
                            foreach (var cardLine in cardLines)
                            {
                                tableauLines.Add(cardLine.Text);
                                tableauLineColorRanges.Add(cardLine.ColorRanges);
                            }
                        }
                    }
                }

                if (tableauLines.Count == 0)
                    tableauLines.Add(Strings.EmptyTableau);

                tableauLines.Insert(0, string.Empty);
                tableauLineColorRanges.Insert(0, new List<ColorRange>());
                int tableauBlockStart = lines.Count;
                AddTableauBlock(lines, tableauLines, PlayerTableauMinimumPanelHeight);
                var tableauColorRangesByLine = new Dictionary<int, List<ColorRange>>();
                for (int lineIndex = 0; lineIndex < tableauLineColorRanges.Count; lineIndex++)
                {
                    if (tableauLineColorRanges[lineIndex].Count > 0)
                        tableauColorRangesByLine[tableauBlockStart + lineIndex + 2] = tableauLineColorRanges[lineIndex];
                }

                panels.Add(new PlayerPanel(lines, storageLine, storageColorRanges, tableauColorRangesByLine));
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
                bool isReservedCardCount = title == Strings.PlayerStorageTitle && i == contentLines.Count - 1;
                lines.Add(FormatPlayerPanelLine(contentLines[i], isReservedCardCount));
            }
            lines.Add(border);
        }

        private static void AddTableauBlock(List<string> lines, List<string> contentLines, int minimumPanelHeight)
        {
            int minimumContentHeight = minimumPanelHeight - lines.Count - 3;
            while (contentLines.Count < minimumContentHeight)
                contentLines.Add(string.Empty);

            AddPlayerBlock(lines, Strings.PlayerTableauTitle, contentLines);
        }

        private static string FormatPlayerPanelLine(string content, bool extraSpaceBeforeRightBorder = false)
        {
            int contentWidth = PlayerPanelContentWidth;
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

        private static List<ColoredLine> FormatTableauCard(DevelopmentCard card, int width)
        {
            string name = card.Name ?? string.Empty;
            var lines = new List<ColoredLine>();
            foreach (string nameLine in WrapText(name, width))
            {
                var lineColorRanges = new List<ColorRange>();
                if (nameLine.Length > 0)
                    lineColorRanges.Add(new ColorRange(0, nameLine.Length, GetRgbColor(card.Bonus)));
                lines.Add(new ColoredLine(nameLine, lineColorRanges));
            }

            if (card.VictoryPoints > 0)
            {
                int lastLineIndex = lines.Count - 1;
                var lastLine = lines[lastLineIndex];
                string separator = lastLine.Text.Length > 0 ? " " : string.Empty;
                string victoryPoints = $"{{{card.VictoryPoints}}}";
                if (lastLine.Text.Length + separator.Length + victoryPoints.Length <= width)
                {
                    lines[lastLineIndex] = lastLine with { Text = lastLine.Text + separator + victoryPoints };
                }
                else
                {
                    lines.Add(new ColoredLine(victoryPoints, new List<ColorRange>()));
                }
            }

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
                3 => Strings.Level3Title,
                2 => Strings.Level2Title,
                1 => Strings.Level1Title,
                _ => string.Empty
            };
        }

        private static RenderedBlock FormatLeader(Leader leader)
        {
            var lines = new string[LeaderBlockHeight];
            var colorRanges = CreateColorRanges(LeaderBlockHeight);
            var requirements = new StringBuilder();
            var requirementColorRanges = new List<ColorRange>();
            AppendBonusNumbers(requirements, requirementColorRanges, leader.Requirements);

            string victoryPoints = leader.VictoryPoints > 0 ? $"{{{leader.VictoryPoints}}}" : string.Empty;
            int requirementWidth = LeaderBlockContentWidth - victoryPoints.Length;
            string requirementSummary = requirements.ToString();
            int colorableRequirementLength = requirementSummary.Length;
            if (requirementSummary.Length > requirementWidth)
            {
                requirementSummary = Ellipsize(requirementSummary, requirementWidth);
                colorableRequirementLength = Math.Max(0, requirementWidth - 3);
            }

            string border = "+" + new string('-', LeaderBlockWidth - 2) + "+";
            string spacer = "|" + new string(' ', LeaderBlockWidth - 2) + "|";
            lines[0] = border;
            lines[1] = "| " + requirementSummary.PadRight(requirementWidth) + victoryPoints + " |";
            lines[2] = spacer;
            lines[3] = CenterCardLine(leader.Name, LeaderBlockContentWidth);
            lines[4] = spacer;
            lines[5] = border;

            var borderColor = new RgbColor(128, 0, 0);
            colorRanges[0].Add(new ColorRange(0, LeaderBlockWidth, borderColor));
            for (int row = 1; row < LeaderBlockHeight - 1; row++)
                AddCardSideBorderRanges(colorRanges, row, LeaderBlockWidth, borderColor);
            colorRanges[LeaderBlockHeight - 1].Add(new ColorRange(0, LeaderBlockWidth, borderColor));

            foreach (var range in requirementColorRanges)
            {
                if (range.Start < colorableRequirementLength)
                {
                    colorRanges[1].Add(range with
                    {
                        Start = range.Start + 2,
                        Length = Math.Min(range.Length, colorableRequirementLength - range.Start)
                    });
                }
            }

            return new RenderedBlock(lines, colorRanges);
        }

        private static List<ColorRange>[] CreateColorRanges(int height)
        {
            var colorRanges = new List<ColorRange>[height];
            for (int row = 0; row < height; row++)
                colorRanges[row] = new List<ColorRange>();
            return colorRanges;
        }

        private static RenderedBlock FormatCard(DevelopmentCard card, int width)
        {
            var lines = new string[DeckBlockHeight];
            var colorRanges = CreateColorRanges(DeckBlockHeight);

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
            AppendBonusNumbers(costText, costColorRanges, cost);
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
