using SplendidWonders.CLI.View;
using SplendidWonders.Core.Model;
using SplendidWonders.Gameplay;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text;

namespace SplendidWonders.Gameplay.Tests.RendererTests
{
    public class ConsoleGameStateRendererTests
    {
        [Test]
        public void Render_TableauSummary_GroupsBonusCountsAndShowsCombinedVictoryPoints()
        {
            const string escape = "\u001b";
            var activeTableau = ImmutableList.Create(
                new DevelopmentCard
                {
                    Id = 1,
                    Name = "Marble Fountain",
                    VictoryPoints = 3,
                    Cost = TokenGroup.FromArray([1, 2, 3, 4, 5, 0]),
                    Bonus = DevelopmentBonus.Green
                },
                new DevelopmentCard
                {
                    Id = 2,
                    Name = "Blue Observatory",
                    VictoryPoints = 0,
                    Cost = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                    Bonus = DevelopmentBonus.Green
                });
            var passiveTableau = ImmutableList.Create(new DevelopmentCard
            {
                Id = 3,
                Name = "Hidden Red Card",
                VictoryPoints = 2,
                Cost = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                Bonus = DevelopmentBonus.Red
            });
            var acquiredLeaders = ImmutableList.Create(new Leader
            {
                Id = 1,
                Name = "Acquired Leader",
                VictoryPoints = 4
            });
            var gameState = GameStateBuilder.Default
                .WithPlayerTableau(1, activeTableau)
                .WithPlayerTableau(2, passiveTableau)
                .WithPlayerAcquiredLeaders(1, acquiredLeaders)
                .Build();
            string[] lines = Render(gameState).Split('\n');
            string renderedOutput = string.Join('\n', lines);
            string activeCount = $"{escape}[38;2;255;255;255;48;2;0;220;50m2{escape}[0m";
            string passiveCount = $"{escape}[38;2;160;160;160;48;2;255;80;80m1{escape}[0m";

            Assert.That(renderedOutput, Does.Contain(activeCount));
            Assert.That(renderedOutput, Does.Contain(passiveCount));
            Assert.That(renderedOutput, Does.Contain("*** Test player 1 ***         {7}"));
            Assert.That(renderedOutput, Does.Contain("Test player 2                   {2}"));
            Assert.That(renderedOutput, Does.Not.Contain("Marble Fountain"));
            Assert.That(renderedOutput, Does.Not.Contain("Hidden Red Card"));
            Assert.That(renderedOutput, Does.Not.Contain("{0}"));
            Assert.That(renderedOutput, Does.Contain($"{escape}[38;2;96;96;96m+----------------------------------+{escape}[0m"));

        }

        [TestCase(3)]
        [TestCase(5)]
        public void Render_Leaders_CreatesCenteredBlocksAboveBoard(int leaderCount)
        {
            const string escape = "\u001b";
            const int leaderBlockWidth = 20;
            const int leaderBlockHeight = 6;
            var leaders = new List<Leader>();
            for (int id = 1; id <= leaderCount; id++)
            {
                leaders.Add(new Leader
                {
                    Id = id,
                    Name = $"Leader {id}",
                    Requirements = new TokenGroup { Yellow = 2, Blue = 3 },
                    VictoryPoints = 4
                });
            }

            var gameState = GameStateBuilder.Default
                .WithLeaders(ImmutableList.CreateRange(leaders))
                .Build();
            string[] lines = Render(gameState).Split('\n');
            var visibleLines = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd('\r');
                visibleLines[i] = StripAnsi(lines[i]);
            }

            string border = "+" + new string('-', leaderBlockWidth - 2) + "+";
            string coloredBorder = $"{escape}[38;2;128;0;0m{border}{escape}[0m";
            string coloredSideBorder = $"{escape}[38;2;128;0;0m|{escape}[0m";
            int titleLineIndex = Array.FindIndex(visibleLines, line => line.Contains("Leader 1", StringComparison.Ordinal));
            Assert.That(titleLineIndex, Is.GreaterThanOrEqualTo(3));

            int topLineIndex = titleLineIndex - 3;
            string topLine = visibleLines[topLineIndex];
            var blockStarts = new List<int>();
            int searchStart = 0;
            for (int i = 0; i < leaderCount; i++)
            {
                int blockStart = topLine.IndexOf(border, searchStart, StringComparison.Ordinal);
                Assert.That(blockStart, Is.GreaterThanOrEqualTo(0));
                blockStarts.Add(blockStart);
                searchStart = blockStart + leaderBlockWidth;
            }

            int rightMargin = topLine.Length - blockStarts[^1] - leaderBlockWidth;
            Assert.That(Math.Abs(blockStarts[0] - rightMargin), Is.LessThanOrEqualTo(1));
            Assert.That(topLine.Substring(blockStarts[0], leaderBlockWidth), Is.EqualTo(border));
            Assert.That(visibleLines[topLineIndex + leaderBlockHeight - 1].Substring(blockStarts[0], leaderBlockWidth), Is.EqualTo(border));
            Assert.That(lines[topLineIndex], Does.Contain(coloredBorder));
            Assert.That(lines[topLineIndex + leaderBlockHeight - 1], Does.Contain(coloredBorder));

            int titleStart = visibleLines[titleLineIndex].IndexOf("Leader 1", blockStarts[0], StringComparison.Ordinal);
            Assert.That(titleStart, Is.EqualTo(blockStarts[0] + 2 + (leaderBlockWidth - 4 - "Leader 1".Length) / 2));

            string requirementLine = visibleLines[topLineIndex + 1];
            string firstRequirementBlock = requirementLine.Substring(blockStarts[0], leaderBlockWidth);
            Assert.That(firstRequirementBlock, Does.StartWith("| "));
            Assert.That(lines[topLineIndex + 1], Does.Contain($"{escape}[38;2;208;208;0m2{escape}[0m"));
            Assert.That(lines[topLineIndex + 1], Does.Contain($"{escape}[38;2;80;145;255m3{escape}[0m"));
            Assert.That(firstRequirementBlock, Does.Contain("{4} |"));
            Assert.That(lines[topLineIndex + 1], Does.Contain(coloredSideBorder));
            Assert.That(lines[titleLineIndex], Does.Contain(coloredSideBorder));
            Assert.That(lines[topLineIndex + 1], Does.Contain($"{{4}} {coloredSideBorder}"));
        }

        private static string Render(GameState gameState)
        {
            var rendererType = typeof(ConsoleGameView).Assembly.GetType(
                "SplendidWonders.CLI.View.ConsoleGameStateRenderer",
                throwOnError: true)!;
            var renderer = Activator.CreateInstance(rendererType, nonPublic: true);
            var render = rendererType.GetMethod("Render", BindingFlags.Instance | BindingFlags.Public)!;
            var originalOutput = Console.Out;
            using var output = new StringWriter();

            try
            {
                Console.SetOut(output);
                render.Invoke(renderer, [gameState]);
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

            return output.ToString();
        }

        private static string StripAnsi(string text)
        {
            var visibleText = new StringBuilder(text.Length);
            bool inEscapeSequence = false;
            foreach (char character in text)
            {
                if (inEscapeSequence)
                {
                    if (character == 'm')
                        inEscapeSequence = false;
                }
                else if (character == '\u001b')
                {
                    inEscapeSequence = true;
                }
                else
                {
                    visibleText.Append(character);
                }
            }

            return visibleText.ToString();
        }
    }
}
