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
                .SetName("Purchase Action - Tableau 42011 - default market + empty store");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 3-long store balanced");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 3-long store yellow-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 3-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store balanced");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store brown-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store only polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 8-long store balanced");

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
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 41) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 89) },
                })
                .SetName("Purchase Action - Tableau 42011 - default market + 8-long store brown-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 8-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 10-long store balanced");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 10-long store red-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 10-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + one reserved card + 5-long store");

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
                .SetName("Purchase Action - Tableau 42011 - default market + multiple reserved cards + 8-long store with polymath");
        }

        private static IEnumerable<TestCaseData> PurchaseActionTableau20233Cases()
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
                .SetName("Purchase Action - Tableau 42011 - default market + empty store");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 3-long store balanced");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 3-long store yellow-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 3-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store balanced");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store brown-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 5-long store only polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 8-long store balanced");

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
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 1) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 2) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 3) },
                    new PurchaseAction() { Card = Cards.Level1.First(c => c.Id == 4) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 41) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 43) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 44) },
                    new PurchaseAction() { Card = Cards.Level2.First(c => c.Id == 89) },
                })
                .SetName("Purchase Action - Tableau 42011 - default market + 8-long store brown-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 8-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 10-long store balanced");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 10-long store red-heavy");

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
                .SetName("Purchase Action - Tableau 42011 - default market + 10-long store with polymath");

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
                .SetName("Purchase Action - Tableau 42011 - default market + one reserved card + 5-long store");

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
                .SetName("Purchase Action - Tableau 42011 - default market + multiple reserved cards + 8-long store with polymath");
        }
    }
}
