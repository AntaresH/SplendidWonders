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
        private const int LeaderBlockWidth = 23;
        private const int LeaderBlockHeight = 8;
        private const int LeaderBlockContentWidth = LeaderBlockWidth - 2;
        private const int LeaderRequirementContentWidth = LeaderBlockContentWidth - 4;
        private const int PlayerPanelWidth = 36;
        private const int PlayerPanelContentWidth = PlayerPanelWidth - 4;
        private const int PlayerPanelSideWidth = 2;

        public void Render(GameState gameState)
        {
            var playerPanels = BuildPlayerPanels(gameState);
            var leaderBlocks = new List<RenderedBlock>();
            foreach (var leader in gameState.Leaders)
                leaderBlocks.Add(FormatLeader(leader));

            int deckTotalHeight = DeckLevelCount * DeckBlockHeight;
            var tokenBlocks = new[]
            {
                (Lines: BuildTokenBlock(gameState.TokenSupply.Yellow), BorderColor: GetRgbColor(DevelopmentBonus.Yellow), BackgroundColor: GetTableauCountBackgroundColor(DevelopmentBonus.Yellow), NumberColor: new RgbColor(255, 255, 255)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Blue), BorderColor: GetRgbColor(DevelopmentBonus.Blue), BackgroundColor: GetTableauCountBackgroundColor(DevelopmentBonus.Blue), NumberColor: new RgbColor(255, 255, 255)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Green), BorderColor: GetRgbColor(DevelopmentBonus.Green), BackgroundColor: GetTableauCountBackgroundColor(DevelopmentBonus.Green), NumberColor: new RgbColor(255, 255, 255)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Brown), BorderColor: GetRgbColor(DevelopmentBonus.Brown), BackgroundColor: GetTableauCountBackgroundColor(DevelopmentBonus.Brown), NumberColor: new RgbColor(255, 255, 255)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Red), BorderColor: GetRgbColor(DevelopmentBonus.Red), BackgroundColor: GetTableauCountBackgroundColor(DevelopmentBonus.Red), NumberColor: new RgbColor(255, 255, 255)),
                (Lines: BuildTokenBlock(gameState.TokenSupply.Polymath, isPolymath: true), BorderColor: new RgbColor(255, 255, 255), BackgroundColor: new RgbColor(255, 255, 255), NumberColor: new RgbColor(96, 96, 96))
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
                {
                    var tokenColorRanges = GetTokenBlockColorRanges(row, block.BorderColor, block.BackgroundColor, block.NumberColor);
                    AppendCanvas(canvas, colorRanges, startRow + row, block.Lines[row].PadRight(TokenColumnWidth), relativeRanges: tokenColorRanges);
                }
            }

            for (int row = 0; row < totalHeight; row++)
                AppendCanvas(canvas, colorRanges, row, new string(' ', Math.Max(0, TokenColumnWidth - canvas[row].Length)) + " ");

            for (int level = DeckLevelCount; level >= 1; level--)
            {
                int startRow = (DeckLevelCount - level) * DeckBlockHeight;
                RgbColor deckColor = GetDeckColor(level);
                string border = "+" + new string('=', DeckColumnWidth - 2) + "+";
                string spacer = (char)0x2551 + new string(' ', DeckColumnWidth - 2) + (char)0x2551;

                for (int row = 0; row < DeckBlockHeight; row++)
                {
                    string title = GetDeckTitle(level);
                    string remainingCount = gameState[level].Remaining.ToString();
                    string? whiteText = row switch
                    {
                        3 => title,
                        5 => remainingCount,
                        _ => null
                    };
                    string line = row switch
                    {
                        0 => border,
                        3 => DeckLineCentered(title, DeckColumnWidth),
                        5 => DeckLineCentered(remainingCount, DeckColumnWidth),
                        DeckBlockHeight - 1 => border,
                        _ => spacer
                    };

                    var deckColorRanges = GetDeckColorRanges(line, row, deckColor, whiteText);
                    AppendCanvas(canvas, colorRanges, startRow + row, line, relativeRanges: deckColorRanges);
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
                    var panelColorRanges = new List<ColorRange>();
                    if (row == playerPanel.StorageLine)
                        panelColorRanges.AddRange(playerPanel.StorageColorRanges);
                    if (playerPanel.TableauColorRanges.TryGetValue(row, out var additionalRanges))
                        panelColorRanges.AddRange(additionalRanges);
                    var canvasColorRanges = new List<ColorRange>(panelColorRanges.Count);
                    foreach (var range in panelColorRanges)
                        canvasColorRanges.Add(range with { Start = range.Start + 3 });
                    AppendCanvas(canvas, colorRanges, row, "   " + panelLine, relativeRanges: canvasColorRanges);
                }
            }

            Console.WriteLine();
            if (leaderBlocks.Count > 0)
            {
                WriteLeaderRow(leaderBlocks, canvas, playerPanels.Count);
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

        private static void AppendTableauCount(StringBuilder text, List<ColorRange> colorRanges, int count, DevelopmentBonus bonus, RgbColor numberColor)
        {
            const int blockWidth = 3;
            if (text.Length > 0)
            {
                colorRanges.Add(new ColorRange(text.Length, 1, null, new RgbColor(0, 0, 0)));
                text.Append(' ');
            }

            string number = count.ToString();
            int leftPadding = (blockWidth - number.Length) / 2;
            int blockStart = text.Length;
            var backgroundColor = GetTableauCountBackgroundColor(bonus);
            colorRanges.Add(new ColorRange(blockStart, blockWidth, numberColor, backgroundColor));
            text.Append(' ', leftPadding).Append(number).Append(' ', blockWidth - leftPadding - number.Length);
        }

        private static void AppendBonusNumbers(StringBuilder text, List<ColorRange> colorRanges, TokenGroup bonuses)
        {
            if (bonuses.Yellow > 0) AppendColoredNumber(text, colorRanges, bonuses.Yellow, GetRgbColor(DevelopmentBonus.Yellow));
            if (bonuses.Blue > 0) AppendColoredNumber(text, colorRanges, bonuses.Blue, GetRgbColor(DevelopmentBonus.Blue));
            if (bonuses.Green > 0) AppendColoredNumber(text, colorRanges, bonuses.Green, GetRgbColor(DevelopmentBonus.Green));
            if (bonuses.Brown > 0) AppendColoredNumber(text, colorRanges, bonuses.Brown, GetRgbColor(DevelopmentBonus.Brown));
            if (bonuses.Red > 0) AppendColoredNumber(text, colorRanges, bonuses.Red, GetRgbColor(DevelopmentBonus.Red));
        }

        private static void AppendLeaderRequirements(StringBuilder text, List<ColorRange> colorRanges, TokenGroup requirements, RgbColor backgroundColor)
        {
            AppendLeaderRequirement(text, colorRanges, requirements.Yellow, DevelopmentBonus.Yellow, backgroundColor);
            AppendLeaderRequirement(text, colorRanges, requirements.Blue, DevelopmentBonus.Blue, backgroundColor);
            AppendLeaderRequirement(text, colorRanges, requirements.Green, DevelopmentBonus.Green, backgroundColor);
            AppendLeaderRequirement(text, colorRanges, requirements.Brown, DevelopmentBonus.Brown, backgroundColor);
            AppendLeaderRequirement(text, colorRanges, requirements.Red, DevelopmentBonus.Red, backgroundColor);
        }

        private static void AppendLeaderRequirement(StringBuilder text, List<ColorRange> colorRanges, int count, DevelopmentBonus bonus, RgbColor backgroundColor)
        {
            const int blockWidth = 3;
            if (count <= 0)
                return;

            if (text.Length > 0)
            {
                colorRanges.Add(new ColorRange(text.Length, 1, null, backgroundColor));
                text.Append(' ');
            }

            string number = count.ToString();
            int leftPadding = (blockWidth - number.Length) / 2;
            int blockStart = text.Length;
            colorRanges.Add(new ColorRange(blockStart, blockWidth, new RgbColor(255, 255, 255), GetTableauCountBackgroundColor(bonus)));
            text.Append(' ', leftPadding).Append(number).Append(' ', blockWidth - leftPadding - number.Length);
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
                    colorRanges[row].Add(range with { Start = start + range.Start });
            }
        }

        private static void WriteCanvasLine(string line, List<ColorRange> ranges)
        {
            ranges.Sort((left, right) => left.Start.CompareTo(right.Start));
            int position = 0;
            foreach (var range in ranges)
            {
                Console.Write(line[position..range.Start]);
                var style = new StringBuilder();
                if (range.Color is not null)
                    style.Append($"38;2;{range.Color.Value.Red};{range.Color.Value.Green};{range.Color.Value.Blue}");
                if (range.Background is not null)
                {
                    if (style.Length > 0)
                        style.Append(';');
                    style.Append($"48;2;{range.Background.Value.Red};{range.Background.Value.Green};{range.Background.Value.Blue}");
                }
                Console.Write($"\x1b[{style}m{line.Substring(range.Start, range.Length)}\x1b[0m");
                position = range.Start + range.Length;
            }

            Console.WriteLine(line[position..]);
        }

        private static void WriteLeaderRow(List<RenderedBlock> leaders, string[] board, int playerPanelCount)
        {
            int boardWidth = 0;
            foreach (string line in board)
                boardWidth = Math.Max(boardWidth, line.Length);

            int widthBeforePlayerPanels = boardWidth - playerPanelCount * (PlayerPanelWidth + 3);
            int rowWidth = Math.Max(widthBeforePlayerPanels, leaders.Count * LeaderBlockWidth);
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
        private readonly record struct ColorRange(int Start, int Length, RgbColor? Color, RgbColor? Background = null);
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
                    AddTableauBlock(lines, new List<string> { string.Empty, Strings.EmptyTableau });
                    panels.Add(new PlayerPanel(lines, -1, new List<ColorRange>(), new Dictionary<int, List<ColorRange>>()));
                    continue;
                }

                string playerName = ReferenceEquals(player, gameState.ActivePlayer)
                    ? $"*** {player.Name} ***"
                    : player.Name;
                int victoryPointTotal = 0;
                if (player.Tableau is not null)
                    foreach (var card in player.Tableau)
                        if (card is not null)
                            victoryPointTotal += card.VictoryPoints;
                if (player.AcquiredLeaders is not null)
                    foreach (var leader in player.AcquiredLeaders)
                        if (leader is not null)
                            victoryPointTotal += leader.VictoryPoints;

                string victoryPoints = victoryPointTotal > 0 ? $"{{{victoryPointTotal}}}" : string.Empty;
                string displayedPlayerName = playerName;
                string playerTitle;
                if (playerName.Length + victoryPoints.Length + 1 <= PlayerPanelContentWidth)
                {
                    playerTitle = playerName.PadRight(PlayerPanelContentWidth - victoryPoints.Length) + victoryPoints;
                }
                else
                {
                    displayedPlayerName = Ellipsize(playerName, PlayerPanelContentWidth - victoryPoints.Length - 1);
                    playerTitle = displayedPlayerName + " " + victoryPoints;
                }
                int nameBlockStart = lines.Count;
                AddPlayerBlock(lines, string.Empty, new List<string> { playerTitle });

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
                storageText.Append("  ");
                var playerStorageLines = new List<string>
                {
                    string.Empty,
                    storageText.ToString()
                };

                int storageLine = lines.Count + 3;
                for (int rangeIndex = 0; rangeIndex < storageColorRanges.Count; rangeIndex++)
                {
                    var range = storageColorRanges[rangeIndex];
                    storageColorRanges[rangeIndex] = range with { Start = range.Start + 2 };
                }

                int storageBlockStart = lines.Count;
                AddPlayerBlock(lines, Strings.PlayerStorageTitle, playerStorageLines);

                var tableauLines = new List<string>();
                var tableauLineColorRanges = new List<List<ColorRange>>();
                int yellowCount = 0;
                int blueCount = 0;
                int greenCount = 0;
                int brownCount = 0;
                int redCount = 0;
                if (player.Tableau is not null)
                {
                    foreach (var card in player.Tableau)
                    {
                        if (card is not null)
                        {
                            switch (card.Bonus)
                            {
                                case DevelopmentBonus.Yellow: yellowCount++; break;
                                case DevelopmentBonus.Blue: blueCount++; break;
                                case DevelopmentBonus.Green: greenCount++; break;
                                case DevelopmentBonus.Brown: brownCount++; break;
                                case DevelopmentBonus.Red: redCount++; break;
                            }
                        }
                    }
                }

                var tableauSummary = new StringBuilder();
                var tableauSummaryColorRanges = new List<ColorRange>();
                var passiveNumberColor = ReferenceEquals(player, gameState.ActivePlayer)
                    ? new RgbColor(255, 255, 255)
                    : new RgbColor(220, 220, 220);
                AppendTableauCount(tableauSummary, tableauSummaryColorRanges, yellowCount, DevelopmentBonus.Yellow, passiveNumberColor);
                AppendTableauCount(tableauSummary, tableauSummaryColorRanges, blueCount, DevelopmentBonus.Blue, passiveNumberColor);
                AppendTableauCount(tableauSummary, tableauSummaryColorRanges, greenCount, DevelopmentBonus.Green, passiveNumberColor);
                AppendTableauCount(tableauSummary, tableauSummaryColorRanges, brownCount, DevelopmentBonus.Brown, passiveNumberColor);
                AppendTableauCount(tableauSummary, tableauSummaryColorRanges, redCount, DevelopmentBonus.Red, passiveNumberColor);
                tableauLines.Add(string.Empty);
                tableauLineColorRanges.Add(new List<ColorRange>());
                tableauLines.Add(tableauSummary.ToString());
                tableauLineColorRanges.Add(tableauSummaryColorRanges);

                int tableauBlockStart = lines.Count;
                AddTableauBlock(lines, tableauLines);
                var tableauColorRangesByLine = new Dictionary<int, List<ColorRange>>();
                if (ReferenceEquals(player, gameState.ActivePlayer))
                {
                    tableauColorRangesByLine[nameBlockStart + 1] = new List<ColorRange>
                    {
                        new ColorRange(2, displayedPlayerName.Length, new RgbColor(255, 255, 128))
                    };
                }
                else
                {
                    var dimTitleColor = new RgbColor(128, 128, 128);
                    AddPanelTitleColorRange(tableauColorRangesByLine, nameBlockStart + 1, displayedPlayerName.Length, dimTitleColor);
                    AddPanelTitleColorRange(tableauColorRangesByLine, storageBlockStart + 1, Math.Min(Strings.PlayerStorageTitle.Length, PlayerPanelContentWidth), dimTitleColor);
                    AddPanelTitleColorRange(tableauColorRangesByLine, tableauBlockStart + 1, Math.Min(Strings.PlayerTableauTitle.Length, PlayerPanelContentWidth), dimTitleColor);
                }

                for (int lineIndex = 0; lineIndex < tableauLineColorRanges.Count; lineIndex++)
                {
                    var ranges = tableauLineColorRanges[lineIndex];
                    for (int rangeIndex = 0; rangeIndex < ranges.Count; rangeIndex++)
                        ranges[rangeIndex] = ranges[rangeIndex] with { Start = ranges[rangeIndex].Start + 2 };
                    int row = tableauBlockStart + lineIndex + 2;
                    if (ranges.Count > 0)
                        tableauColorRangesByLine[row] = ranges;
                }

                if (!ReferenceEquals(player, gameState.ActivePlayer))
                {
                    var dimBorder = new RgbColor(96, 96, 96);
                    AddDimPanelBorderRanges(tableauColorRangesByLine, nameBlockStart, nameBlockStart + 2, dimBorder);
                    AddDimPanelBorderRanges(tableauColorRangesByLine, storageBlockStart, tableauBlockStart - 1, dimBorder, storageLine);
                    AddDimPanelBorderRanges(tableauColorRangesByLine, tableauBlockStart, lines.Count - 1, dimBorder);
                }

                panels.Add(new PlayerPanel(lines, storageLine, storageColorRanges, tableauColorRangesByLine));
            }

            return panels;
        }

        private static void AddPanelTitleColorRange(Dictionary<int, List<ColorRange>> colorRangesByLine, int row, int length, RgbColor color)
        {
            if (length <= 0)
                return;

            if (!colorRangesByLine.TryGetValue(row, out var ranges))
                colorRangesByLine[row] = ranges = new List<ColorRange>();
            ranges.Add(new ColorRange(2, length, color));
        }

        private static void AddDimPanelBorderRanges(Dictionary<int, List<ColorRange>> colorRangesByLine, int blockStart, int blockEnd, RgbColor color, int extendedRightBorderRow = -1)
        {
            for (int row = blockStart; row <= blockEnd; row++)
            {
                if (!colorRangesByLine.TryGetValue(row, out var ranges))
                    colorRangesByLine[row] = ranges = new List<ColorRange>();

                if (row == blockStart || row == blockEnd)
                    ranges.Add(new ColorRange(0, PlayerPanelWidth, color));
                else
                {
                    ranges.Add(new ColorRange(0, 1, color));
                    int rightBorderPosition = row == extendedRightBorderRow
                        ? PlayerPanelWidth
                        : PlayerPanelWidth - 1;
                    ranges.Add(new ColorRange(rightBorderPosition, 1, color));
                }
            }
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

        private static void AddTableauBlock(List<string> lines, List<string> contentLines)
        {
            AddPlayerBlock(lines, Strings.PlayerTableauTitle, contentLines);
        }

        private static string FormatPlayerPanelLine(string content, bool extraSpaceBeforeRightBorder = false)
        {
            int contentWidth = extraSpaceBeforeRightBorder
                ? PlayerPanelContentWidth - 1
                : PlayerPanelContentWidth;
            string panelContent = content.Length > contentWidth
                ? content[..contentWidth]
                : content.PadRight(contentWidth);
            string rightBorder = extraSpaceBeforeRightBorder ? "   |" : " |";
            return "| " + panelContent + rightBorder;
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

        private static List<ColorRange> GetTokenBlockColorRanges(int row, RgbColor borderColor, RgbColor backgroundColor, RgbColor numberColor)
        {
            var colorRanges = new List<ColorRange>();
            if (row == 0 || row == TokenBlockHeight - 1)
            {
                colorRanges.Add(new ColorRange(2, TokenColumnWidth - 4, borderColor));
                return colorRanges;
            }

            colorRanges.Add(new ColorRange(1, 1, borderColor));
            colorRanges.Add(new ColorRange(TokenColumnWidth - 2, 1, borderColor));
            colorRanges.Add(new ColorRange(2, TokenColumnWidth - 4, numberColor, backgroundColor));
            return colorRanges;
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

        private static RgbColor GetDeckColor(int level)
        {
            return level switch
            {
                1 => new RgbColor(204, 85, 0),
                2 => new RgbColor(0, 128, 128),
                3 => new RgbColor(160, 0, 80),
                _ => new RgbColor(96, 96, 96)
            };
        }

        private static List<ColorRange> GetDeckColorRanges(string line, int row, RgbColor deckColor, string? whiteText)
        {
            var colorRanges = new List<ColorRange>();
            if (row == 0 || row == DeckBlockHeight - 1)
            {
                colorRanges.Add(new ColorRange(0, line.Length, deckColor));
                return colorRanges;
            }

            colorRanges.Add(new ColorRange(0, 1, deckColor));
            colorRanges.Add(new ColorRange(line.Length - 1, 1, deckColor));

            if (whiteText is null)
            {
                colorRanges.Add(new ColorRange(1, line.Length - 2, null, deckColor));
                return colorRanges;
            }

            int textStart = line.IndexOf(whiteText, StringComparison.Ordinal);
            if (textStart < 1)
            {
                colorRanges.Add(new ColorRange(1, line.Length - 2, null, deckColor));
                return colorRanges;
            }

            int textEnd = textStart + whiteText.Length;
            if (textStart > 1)
                colorRanges.Add(new ColorRange(1, textStart - 1, null, deckColor));
            colorRanges.Add(new ColorRange(textStart, whiteText.Length, new RgbColor(255, 255, 255), deckColor));
            if (textEnd < line.Length - 1)
                colorRanges.Add(new ColorRange(textEnd, line.Length - 1 - textEnd, null, deckColor));

            return colorRanges;
        }

        private static RenderedBlock FormatLeader(Leader leader)
        {
            var lines = new string[LeaderBlockHeight];
            var colorRanges = CreateColorRanges(LeaderBlockHeight);
            var backgroundColor = new RgbColor(80, 0, 0);
            var requirements = new StringBuilder();
            var requirementColorRanges = new List<ColorRange>();
            AppendLeaderRequirements(requirements, requirementColorRanges, leader.Requirements, backgroundColor);

            string victoryPoints = leader.VictoryPoints > 0 ? $"{{{leader.VictoryPoints}}}" : string.Empty;
            int requirementWidth = LeaderRequirementContentWidth - victoryPoints.Length;
            string requirementSummary = requirements.ToString();
            int colorableRequirementLength = requirementSummary.Length;
            if (requirementSummary.Length > requirementWidth)
            {
                requirementSummary = Ellipsize(requirementSummary, requirementWidth);
                colorableRequirementLength = Math.Max(0, requirementWidth - 3);
            }

            var white = new RgbColor(255, 255, 255);
            string border = "+" + new string('=', LeaderBlockWidth - 2) + "+";
            string title = CenterText("♛  " + (leader.Name ?? string.Empty), LeaderBlockContentWidth);
            string requirementsLine = new string(' ', 2) + requirementSummary.PadRight(requirementWidth) + victoryPoints + new string(' ', 2);
            var contentLines = new[]
            {
                new string(' ', LeaderBlockContentWidth),
                title,
                new string(' ', LeaderBlockContentWidth),
                new string(' ', LeaderBlockContentWidth),
                requirementsLine,
                new string(' ', LeaderBlockContentWidth)
            };

            lines[0] = border;
            for (int row = 0; row < contentLines.Length; row++)
                lines[row + 1] = (char)0x2551 + contentLines[row] + (char)0x2551;
            lines[LeaderBlockHeight - 1] = border;

            var borderColor = backgroundColor;
            colorRanges[0].Add(new ColorRange(0, LeaderBlockWidth, borderColor));
            colorRanges[LeaderBlockHeight - 1].Add(new ColorRange(0, LeaderBlockWidth, borderColor));
            for (int row = 1; row < LeaderBlockHeight - 1; row++)
            {
                AddCardSideBorderRanges(colorRanges, row, LeaderBlockWidth, borderColor);
                if (row - 1 == 4)
                    AddLeaderRequirementLineColorRanges(colorRanges[row], requirementColorRanges, colorableRequirementLength, requirementWidth, victoryPoints, backgroundColor);
                else
                    colorRanges[row].Add(new ColorRange(1, LeaderBlockContentWidth, white, backgroundColor));
            }

            return new RenderedBlock(lines, colorRanges);
        }

        private static void AddLeaderRequirementLineColorRanges(List<ColorRange> colorRanges, List<ColorRange> requirementColorRanges, int colorableRequirementLength, int requirementWidth, string victoryPoints, RgbColor backgroundColor)
        {
            int backgroundStart = 1;
            foreach (var range in requirementColorRanges)
            {
                if (range.Start >= colorableRequirementLength)
                    continue;

                int textStart = range.Start + 3;
                int textLength = Math.Min(range.Length, colorableRequirementLength - range.Start);
                if (textStart > backgroundStart)
                    colorRanges.Add(new ColorRange(backgroundStart, textStart - backgroundStart, null, backgroundColor));
                colorRanges.Add(range with { Start = textStart, Length = textLength });
                backgroundStart = textStart + textLength;
            }

            int victoryPointsStart = 3 + requirementWidth;
            if (victoryPointsStart > backgroundStart)
                colorRanges.Add(new ColorRange(backgroundStart, victoryPointsStart - backgroundStart, null, backgroundColor));
            if (victoryPoints.Length > 0)
            {
                colorRanges.Add(new ColorRange(victoryPointsStart, victoryPoints.Length, new RgbColor(255, 255, 255), backgroundColor));
                backgroundStart = victoryPointsStart + victoryPoints.Length;
            }
            else
            {
                backgroundStart = victoryPointsStart;
            }

            if (backgroundStart < LeaderBlockWidth - 1)
                colorRanges.Add(new ColorRange(backgroundStart, LeaderBlockWidth - 1 - backgroundStart, null, backgroundColor));
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
            bool titleUsesTwoLines = nameLines.Count == 2;

            while (nameLines.Count < 3)
                nameLines.Add(string.Empty);

            RgbColor cardBackground = GetTableauCountBackgroundColor(card.Bonus);
            RgbColor bonusColor = cardBackground;
            string victoryPoints = card.VictoryPoints > 0 ? "{" + card.VictoryPoints + "}" : string.Empty;
            var cost = card.Cost;
            var costText = new StringBuilder();
            var costColorRanges = new List<ColorRange>();
            AppendBonusNumbers(costText, costColorRanges, cost);
            string costSummary = costText.ToString();
            if (victoryPoints.Length > contentWidth) victoryPoints = victoryPoints.Substring(0, contentWidth);
            int requirementsWidth = contentWidth - victoryPoints.Length;
            if (costSummary.Length > requirementsWidth)
                costSummary = Ellipsize(costSummary, requirementsWidth);
            var visibleCostRanges = new List<ColorRange>();
            foreach (var range in costColorRanges)
            {
                if (range.Start < costSummary.Length)
                    visibleCostRanges.Add(range with { Length = Math.Min(range.Length, costSummary.Length - range.Start) });
            }

            string requirementsContent = costSummary.PadRight(requirementsWidth) + victoryPoints;

            lines[0] = top;
            lines[1] = "|" + new string(' ', width - 2) + "|";
            lines[2] = CenterCardLine(nameLines[0], contentWidth);
            lines[3] = CenterCardLine(nameLines[1], contentWidth);
            lines[4] = CenterCardLine(nameLines[2], contentWidth);
            lines[5] = "|" + new string(' ', width - 2) + "|";
            lines[6] = "| " + requirementsContent.PadRight(contentWidth) + " |";
            lines[7] = top;

            colorRanges[0].Add(new ColorRange(0, width, bonusColor));
            AddCardSideBorderRanges(colorRanges, 1, width, bonusColor);
            AddCardInteriorBackground(colorRanges[1], width, cardBackground);
            for (int row = 2; row <= 5; row++)
                AddCardSideBorderRanges(colorRanges, row, width, bonusColor);
            for (int row = 0; row < nameLines.Count; row++)
            {
                int titleStart = 2 + (contentWidth - nameLines[row].Length) / 2;
                RgbColor titleBackground = row == 2 && !titleUsesTwoLines ? new RgbColor(0, 0, 0) : cardBackground;
                AddCardInteriorBackground(colorRanges[row + 2], width, titleBackground, titleStart, nameLines[row].Length, new RgbColor(255, 255, 255));
            }
            AddCardInteriorBackground(colorRanges[5], width, new RgbColor(0, 0, 0));
            AddCardSideBorderRanges(colorRanges, 6, width, bonusColor);
            int victoryPointsStart = 2 + requirementsWidth;
            AddCardCostRanges(colorRanges[6], width, visibleCostRanges, victoryPointsStart, victoryPoints);
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

        private static void AddCardInteriorBackground(List<ColorRange> colorRanges, int width, RgbColor backgroundColor, int textStart = -1, int textLength = 0, RgbColor? textColor = null)
        {
            if (textLength <= 0)
            {
                colorRanges.Add(new ColorRange(1, width - 2, null, backgroundColor));
                return;
            }

            if (textStart > 1)
                colorRanges.Add(new ColorRange(1, textStart - 1, null, backgroundColor));
            colorRanges.Add(new ColorRange(textStart, textLength, textColor, backgroundColor));
            int textEnd = textStart + textLength;
            if (textEnd < width - 1)
                colorRanges.Add(new ColorRange(textEnd, width - 1 - textEnd, null, backgroundColor));
        }

        private static void AddCardCostRanges(List<ColorRange> colorRanges, int width, List<ColorRange> visibleCostRanges, int victoryPointsStart, string victoryPoints)
        {
            int backgroundStart = 1;
            RgbColor black = new(0, 0, 0);
            foreach (var range in visibleCostRanges)
            {
                int textStart = range.Start + 2;
                if (textStart > backgroundStart)
                    colorRanges.Add(new ColorRange(backgroundStart, textStart - backgroundStart, null, black));
                colorRanges.Add(range with { Start = textStart, Background = black });
                backgroundStart = textStart + range.Length;
            }

            if (victoryPoints.Length > 0)
            {
                if (victoryPointsStart > backgroundStart)
                    colorRanges.Add(new ColorRange(backgroundStart, victoryPointsStart - backgroundStart, null, black));
                colorRanges.Add(new ColorRange(victoryPointsStart, victoryPoints.Length, new RgbColor(255, 255, 255), black));
                backgroundStart = victoryPointsStart + victoryPoints.Length;
            }

            if (backgroundStart < width - 1)
                colorRanges.Add(new ColorRange(backgroundStart, width - 1 - backgroundStart, null, black));
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
                DevelopmentBonus.Red => new RgbColor(220, 80, 80),
                _ => new RgbColor(192, 192, 192)
            };
        }

        private static RgbColor GetTableauCountBackgroundColor(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => new RgbColor(138, 116, 0),
                DevelopmentBonus.Blue => new RgbColor(30, 60, 165),
                DevelopmentBonus.Green => new RgbColor(0, 125, 30),
                DevelopmentBonus.Brown => new RgbColor(100, 45, 0),
                DevelopmentBonus.Red => new RgbColor(180, 10, 20),
                _ => new RgbColor(96, 96, 96)
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
