using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using NUnit.Framework.Interfaces;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SplendidWonders.Gameplay.Tests
{
    public partial class ActionExplorerTests
    {

        private static IEnumerable<TestCaseData> SelectLeaderActionCases()
        {
            var ybgLeader = Leaders.StandardLeaders.First(leader => leader.Id == 6);
            var gorLeader = Leaders.StandardLeaders.First(leader => leader.Id == 8);
            var ybrLeader = Leaders.StandardLeaders.First(leader => leader.Id == 10);
            var ygrLeader = Leaders.ExtendedLeaders.First(leader => leader.Id == 20);

            var ybLeader = Leaders.StandardLeaders.First(leader => leader.Id == 1);
            var bgLeader = Leaders.StandardLeaders.First(leader => leader.Id == 2);
            var ygLeader = Leaders.ExtendedLeaders.First(leader => leader.Id == 11);

            var twoWaySelectionLeaders333 = new List<Leader>
            {
                ybgLeader, gorLeader, ybrLeader
            }.ToImmutableList();

            var threeWaySelectionLeaders333 = new List<Leader>
            {
                ybgLeader,
                ybrLeader,
                ygrLeader
            }.ToImmutableList();

            var twoWaySelectionLeaders44 = new List<Leader>
            {
                ybLeader, ygLeader, ybgLeader
            }.ToImmutableList();

            var threeWaySelectionLeaders44 = new List<Leader>
            {
                ybLeader,
                ygLeader,
                bgLeader,
                gorLeader,
                ygrLeader
            }.ToImmutableList();

            var playerTableau333 = new List<DevelopmentCard>();

            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(3).ToList());
            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(3).ToList());
            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(3).ToList());
            playerTableau333.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Red).Take(3).ToList());


            var playerTableau44 = new List<DevelopmentCard>();

            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(4).ToList());
            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(4).ToList());
            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(4).ToList());
            playerTableau44.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Brown).Take(4).ToList());

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau333.ToImmutableList()
                    }
                },
                twoWaySelectionLeaders333,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybgLeader },
                    new SelectLeaderAction() { Leader = ybrLeader }
                })
                .SetName("Select Leader - Two 3-3-3 leaders are eligible");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau333.ToImmutableList()
                    }
                },
                threeWaySelectionLeaders333,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybgLeader },
                    new SelectLeaderAction() { Leader = ybrLeader },
                    new SelectLeaderAction() { Leader = ygrLeader }
                })
                .SetName("Select Leader - Three 3-3-3 leaders are eligible");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau44.ToImmutableList()
                    }
                },
                twoWaySelectionLeaders44,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybLeader },
                    new SelectLeaderAction() { Leader = ygLeader }
                })
                .SetName("Select Leader - Two 4-4 leaders are eligible");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Tableau = playerTableau44.ToImmutableList()
                    }
                },
                threeWaySelectionLeaders44,
                new[]
                {
                    new SelectLeaderAction() { Leader = ybLeader },
                    new SelectLeaderAction() { Leader = ygLeader },
                    new SelectLeaderAction() { Leader = bgLeader }
                })
                .SetName("Select Leader - Three 4-4 leaders are eligible");
        }

        private static IEnumerable<TestCaseData> PurchaseActionEmptyTableauCases()
        {
            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0])
                    }
                },
                new PurchaseAction[] { })
                .SetName("Purchase Action - no tokens + empty tableau + default card setup");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([3, 0, 0, 1, 0, 0])
                    }
                },
                new PurchaseAction[] { })
                .SetName("Purchase Action - Empty Tableau - not suitable tokens");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 2, 2, 0, 1, 0])
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) }
                })
                .SetName("Purchase Action - Empty Tableau - 2 blue, 2 green, 1 red");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 2, 1, 0])
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) }
                })
                .SetName("Purchase Action - Empty Tableau - 2 brown, 1 red");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 2, 0, 1])
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) }
                })
                .SetName("Purchase Action -Empty Tableau -  2 brown, 1 polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 2])
                    }
                },
                new PurchaseAction[] { })
                .SetName("Purchase Action - Empty Tableau - 2 polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 3])
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) }
                })
                .SetName("Purchase Action - Empty Tableau - 3 polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([3, 4, 1, 0, 1, 1])
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) }
                })
                .SetName("Purchase Action - Empty Tableau - lots of tokens #1");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 1, 5, 2, 0])
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) }
                })
                .SetName("Purchase Action - Empty Tableau - lots of tokens #2");
        }
    }
}
