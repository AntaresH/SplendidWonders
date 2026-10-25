using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using NUnit.Framework.Interfaces;
using SplendidWonders.Core.Model;
using SplendidWonders.Core.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Xml.Linq;

namespace SplendidWonders.Gameplay.Tests
{
    public partial class ActionExplorerTests
    {
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
                .SetName("Purchase Action - Empty Tableau - no tokens");

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
                .SetName("Purchase Action - Empty Tableau -  2 brown, 1 polymath");

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
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
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

        private static IEnumerable<TestCaseData> PurchaseActionTableau42011Cases()
        {
            var playerTableau42011 = new List<DevelopmentCard>();

            playerTableau42011.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(4).ToList());
            playerTableau42011.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Blue).Take(2).ToList());
            playerTableau42011.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Brown).Take(1).ToList());
            playerTableau42011.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Red).Take(1).ToList());

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },

                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - empty store");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 1, 1, 0, 1, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },

                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 + 3-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([3, 0, 0, 0, 0, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 + 3-long store yellow-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 0, 1, 0, 0, 1]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },

                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 3-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 1, 1, 1, 1, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },

                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 5-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 5, 0, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },

                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 5-long store brown-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 5-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 5]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) }
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 5-long store only polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([2, 1, 2, 1, 2, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 8-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 1, 0, 5, 2, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 41) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 8-long store brown-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([3, 0, 0, 2, 1, 2]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 8-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([2, 1, 2, 3, 2, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 10-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 1, 0, 2, 6, 0]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 71) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 72) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 74) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 10-long store red-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 1, 2, 2, 2, 2]),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 41) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - 10-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 5, 0, 0, 0, 0]),
                        ReservedCards = new List<DevelopmentCard>()
                        {
                            Cards.Level3.First(c => c.Id == 81),

                        }.ToImmutableList(),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 81) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - one reserved card + 5-long store");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 1, 0, 2, 2, 2]),
                        ReservedCards = new List<DevelopmentCard>()
                        {
                            Cards.Level1.First(c => c.Id == 32),
                            Cards.Level3.First(c => c.Id == 82),

                        }.ToImmutableList(),
                        Tableau = playerTableau42011.ToImmutableList()
                    }
                },
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 41) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 42) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 32) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 82) },
                })
                .SetName("Purchase Action - Tableau 42011 - Card Setup #0 - multiple reserved cards + 8-long store with polymath");
        }

        private static IEnumerable<TestCaseData> PurchaseActionTableau20233CardSeed12345Cases()
        {
            var playerTableau20233 = new List<DevelopmentCard>();

            playerTableau20233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Yellow).Take(2).ToList());
            playerTableau20233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Green).Take(2).ToList());
            playerTableau20233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Brown).Take(3).ToList());
            playerTableau20233.AddRange(Cards.Level1.Where(card => card.Bonus == DevelopmentBonus.Red).Take(3).ToList());

            int testSeed = 12345;

            var seededCardSetup = GameFactory.GetRandomizedCardSetup(testSeed);

            // Level 1
            //+ [0] { DevelopmentCard { Id = 3, Name = Kereskedőház, VictoryPoints = 0, Cost = TokenGroup { Yellow = 3, Blue = 1, Green = 0, Brown = 0, Red = 1, Polymath = 0 }, Bonus = Yellow } }
            //+ [1] { DevelopmentCard { Id = 2, Name = Borászat, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 3, Green = 0, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Yellow } }
            //+ [2] { DevelopmentCard { Id = 18, Name = Kórház, VictoryPoints = 1, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 0, Red = 4, Polymath = 0 }, Bonus = Green } }
            //+ [3] { DevelopmentCard { Id = 6, Name = Piac, VictoryPoints = 0, Cost = TokenGroup { Yellow = 0, Blue = 1, Green = 1, Brown = 1, Red = 1, Polymath = 0 }, Bonus = Yellow } }

            // Level 2
            //+ [0] { DevelopmentCard { Id = 56, Name = Matematika, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 6, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [1] { DevelopmentCard { Id = 50, Name = Képzőművészet, VictoryPoints = 2, Cost = TokenGroup { Yellow = 5, Blue = 3, Green = 0, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Blue } }
            //+ [2] { DevelopmentCard { Id = 47, Name = Filozófia, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 6, Green = 0, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Blue } }
            //+ [3] { DevelopmentCard { Id = 62, Name = Mérnökség, VictoryPoints = 3, Cost = TokenGroup { Yellow = 0, Blue = 0, Green = 0, Brown = 6, Red = 0, Polymath = 0 }, Bonus = Brown } }

            // Level 3
            //+ [0] { DevelopmentCard { Id = 71, Name = Alexandriai Világítótorony, VictoryPoints = 4, Cost = TokenGroup { Yellow = 3, Blue = 0, Green = 0, Brown = 3, Red = 6, Polymath = 0 }, Bonus = Yellow } }
            //+ [1] { DevelopmentCard { Id = 79, Name = Alexandriai Nagy Könyvtár, VictoryPoints = 5, Cost = TokenGroup { Yellow = 0, Blue = 7, Green = 3, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Green } }
            //+ [2] { DevelopmentCard { Id = 82, Name = Uri Nagy Ziggurat, VictoryPoints = 3, Cost = TokenGroup { Yellow = 5, Blue = 3, Green = 0, Brown = 2, Red = 3, Polymath = 0 }, Bonus = Green } }
            //+ [3] { DevelopmentCard { Id = 76, Name = Epheszoszi Artemisz Templom, VictoryPoints = 5, Cost = TokenGroup { Yellow = 7, Blue = 3, Green = 0, Brown = 0, Red = 0, Polymath = 0 }, Bonus = Blue } }

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new PurchaseAction[] { })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 - empty store");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 1, 0, 0, 1, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },

                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 3-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 3, 0, 0, 0, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 3-long store blue-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 1, 1, 0, 0, 1]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },

                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 3-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 1, 1, 1, 1, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },

                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 5-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 1, 0, 4, 0, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 62) },

                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 5-long store brown-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([2, 0, 0, 0, 1, 2]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 71) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 5-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 0, 0, 0, 0, 5]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 56) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 62) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 71) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 5-long store only polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([2, 1, 2, 1, 2, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 8-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 6, 0, 0, 1, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 47) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 8-long store blue-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([3, 0, 0, 2, 1, 2]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 62) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 71) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 8-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([3, 3, 2, 1, 1, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 50) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 82) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 10-long store balanced");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([5, 3, 1, 0, 1, 0]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 50) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 76) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 82) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 10-long store yellow-heavy");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 0, 3, 2, 2, 2]),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 56) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 62) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 71) }
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + 10-long store with polymath");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([0, 5, 0, 0, 0, 0]),
                        ReservedCards = new List<DevelopmentCard>()
                        {
                            Cards.Level2.First(c => c.Id == 49),

                        }.ToImmutableList(),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                },
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 49) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + one reserved card + 5-long store");

            yield return new TestCaseData(

                new List<Player>()
                {
                    new Player()
                    {
                        Name = "Test player 1",
                        Store = TokenGroup.FromArray([1, 1, 0, 2, 2, 2]),
                        ReservedCards = new List<DevelopmentCard>()
                        {
                            Cards.Level1.First(c => c.Id == 36),
                            Cards.Level2.First(c => c.Id == 51),
                            Cards.Level3.First(c => c.Id == 88),
                        }.ToImmutableList(),
                        Tableau = playerTableau20233.ToImmutableList()
                    }
                }, 
                seededCardSetup,
                new[]
                {
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 6) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 18) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 62) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 71) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 36) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 51) },
                    new PurchaseAction() { Card = Cards.Level3.First(c => c.Id == 88) },
                })
                .SetName("Purchase Action - Tableau 20233 - Card Setup #12345 + multiple reserved cards + 8-long store with polymath");
        }
    }
}
