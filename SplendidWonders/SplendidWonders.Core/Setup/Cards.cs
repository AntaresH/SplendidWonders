using SplendidWonders.Core.Model;
using SplendidWonders.Core.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public static class Cards
    {
        public static readonly IReadOnlyList<DevelopmentCard> Level1 = new List<DevelopmentCard>
        {
            new DevelopmentCard { Id = 1, Name = Resources.Card_Birosag, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 2, Green = 2, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 2, Name = Resources.Card_Boraszat, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 3, Name = Resources.Card_Kereskedohaz, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 3, Blue = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 4, Name = Resources.Card_Kikoto, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Brown = 2, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 5, Name = Resources.Card_Magtar, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 1, Green = 2, Brown = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 6, Name = Resources.Card_Piac, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 1, Green = 1, Brown = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 7, Name = Resources.Card_Tanacs, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 2, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 8, Name = Resources.Card_Varoshaza, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Green = 4 }, VictoryPoints = 0 },

            new DevelopmentCard { Id = 9, Name = Resources.Card_Cirkusz, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Red = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 10, Name = Resources.Card_Furdo, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 1, Green = 3, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 11, Name = Resources.Card_Kertek, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Green = 2, Brown = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 12, Name = Resources.Card_Palota, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Brown = 4 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 13, Name = Resources.Card_Sirkamra, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 14, Name = Resources.Card_Szentely, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Green = 2, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 15, Name = Resources.Card_Szinhaz, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Green = 1, Brown = 2, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 16, Name = Resources.Card_Templom, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Green = 1, Brown = 1, Red = 1 }, VictoryPoints = 0 },

            new DevelopmentCard { Id = 17, Name = Resources.Card_Iskola, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 1, Blue = 1, Brown = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 18, Name = Resources.Card_Korhaz, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Red = 4 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 19, Name = Resources.Card_Konyvtar, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 1, Blue = 3, Green = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 20, Name = Resources.Card_Laboratorium, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 1, Blue = 1, Brown = 1, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 21, Name = Resources.Card_Leveltar, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 2, Blue = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 22, Name = Resources.Card_Papiruszgyar, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 2, Brown = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 23, Name = Resources.Card_Patika, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 1, Brown = 2, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 24, Name = Resources.Card_Uveggyar, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Brown = 3 }, VictoryPoints = 0 },

            new DevelopmentCard { Id = 25, Name = Resources.Card_Acsmuhely, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Green = 1, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 26, Name = Resources.Card_Banya, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 27, Name = Resources.Card_Furesztelep, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Blue = 2, Green = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 28, Name = Resources.Card_Hajogyar, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Brown = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 29, Name = Resources.Card_Kofejto, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 1, Blue = 1, Green = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 30, Name = Resources.Card_Ontode, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 1, Brown = 1, Red = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 31, Name = Resources.Card_Teglagyar, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Blue = 1, Green = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 32, Name = Resources.Card_Vizvezetek, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 4 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 33, Name = Resources.Card_Arena, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Blue = 4 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 34, Name = Resources.Card_Erod, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 1, Brown = 3, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 35, Name = Resources.Card_Fegyverkovacs, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 2, Blue = 2, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 36, Name = Resources.Card_Helyors, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 2, Green = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 37, Name = Resources.Card_Istallo, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 38, Name = Resources.Card_Laktanya, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 2, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 39, Name = Resources.Card_Ortorony, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 1, Blue = 1, Green = 1, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 40, Name = Resources.Card_Varosfal, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 1, Blue = 2, Green = 1, Brown = 1 }, VictoryPoints = 0 },
        };

        public static readonly IReadOnlyList<DevelopmentCard> Level2 = new List<DevelopmentCard>
        {
            new DevelopmentCard { Id = 41, Name = Resources.Card_Allamigazgatas, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Brown = 5, Red = 3 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 42, Name = Resources.Card_Koztarsasag, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 43, Name = Resources.Card_Monarchia, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Green = 1, Brown = 4, Red = 2 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 44, Name = Resources.Card_Penzveres, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Brown = 5}, VictoryPoints = 2 },
            new DevelopmentCard { Id = 45, Name = Resources.Card_Tengereszet, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Green = 3, Brown = 2, Red = 2 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 46, Name = Resources.Card_Torvenykonyv, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 2, Blue = 3, Brown = 3 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 47, Name = Resources.Card_Filozofia, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 48, Name = Resources.Card_Iparmuveszet, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 2, Green = 3, Brown = 3}, VictoryPoints = 1 },
            new DevelopmentCard { Id = 49, Name = Resources.Card_Irodalom, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 50, Name = Resources.Card_Kepzomuveszet, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 5, Blue = 3}, VictoryPoints = 2 },
            new DevelopmentCard { Id = 51, Name = Resources.Card_Misztika, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 2, Green = 3, Red = 3 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 52, Name = Resources.Card_Zene, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 2, Brown = 1, Red = 4 }, VictoryPoints = 2 },

            new DevelopmentCard { Id = 53, Name = Resources.Card_Alkimia, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Green = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 54, Name = Resources.Card_Abece, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 5, Green = 3 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 55, Name = Resources.Card_Csillagaszat, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 4, Blue = 2, Red = 1 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 56, Name = Resources.Card_Matematika, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Green = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 57, Name = Resources.Card_Naptar, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 3, Green = 2, Brown = 3 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 58, Name = Resources.Card_Terkepeszet, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 2, Blue = 3, Red = 2 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 59, Name = Resources.Card_Epiteszet, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 1, Blue = 4, Green = 2 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 60, Name = Resources.Card_Hidepites, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 3, Red = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 61, Name = Resources.Card_Mezogazdasag, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Brown = 2, Red = 3 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 62, Name = Resources.Card_Mernokseg, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Brown = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 63, Name = Resources.Card_Utrendszer, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Red = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 64, Name = Resources.Card_Varostervezes, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Blue = 3, Brown = 2, Red = 3 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 65, Name = Resources.Card_Armada, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 66, Name = Resources.Card_HarciSzekerek, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 3, Blue = 2, Green = 2 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 67, Name = Resources.Card_LovasIjaszat, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 3, Green = 3, Red = 2 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 68, Name = Resources.Card_Ostromszakertelem, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 5, Brown = 3 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 69, Name = Resources.Card_Hadvezetes, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Red = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 70, Name = Resources.Card_Vasmuvesseg, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Blue = 1, Green = 4, Brown = 2 }, VictoryPoints = 2 },
        };

        public static IReadOnlyList<DevelopmentCard> Level3 = new List<DevelopmentCard>
        {
            new DevelopmentCard { Id = 71, Name = Resources.Card_AlexandriaiVilagitotorony, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 3, Brown = 3, Red = 6 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 72, Name = Resources.Card_Perszepolisz, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 3, Red = 7 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 73, Name = Resources.Card_Petra, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 2, Green = 3, Brown = 5, Red = 3 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 74, Name = Resources.Card_RodosziKolosszus, Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Red = 7 }, VictoryPoints = 4 },

            new DevelopmentCard { Id = 75, Name = Resources.Card_Agyaghadsereg, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 3, Green = 2, Brown = 3, Red = 5 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 76, Name = Resources.Card_EpheszosziArtemiszTemplom, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 7, Blue = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 77, Name = Resources.Card_HalikarnasszosziMauzoleum, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 7 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 78, Name = Resources.Card_KnosszosziPalota, Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 6, Blue = 3, Red = 3 }, VictoryPoints = 4 },

            new DevelopmentCard { Id = 79, Name = Resources.Card_AlexandriaiNagyKonyvtar, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 7, Green = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 80, Name = Resources.Card_GundesapurEgyetem, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 3, Blue = 6, Green = 3 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 81, Name = Resources.Card_NalandaKolostor, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 7 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 82, Name = Resources.Card_UriNagyZiggurat, Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 5, Blue = 3, Brown = 2, Red = 3 }, VictoryPoints = 3 },

            new DevelopmentCard { Id = 83, Name = Resources.Card_BabiloniFuggokertek, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Green = 7 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 84, Name = Resources.Card_GizaiPiramisok, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Green = 7, Brown = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 85, Name = Resources.Card_HagiaSophia, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Blue = 3, Green = 6, Brown = 3 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 86, Name = Resources.Card_Pantheon, Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 3, Blue = 5, Green = 3, Red = 2 }, VictoryPoints = 3 },

            new DevelopmentCard { Id = 87, Name = Resources.Card_IstarKapuja, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 2, Blue = 3, Green = 5, Brown = 3 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 88, Name = Resources.Card_KinaiNagyFal, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Brown = 7, Red = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 89, Name = Resources.Card_Kolosszeum, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 3, Brown = 6, Red = 3 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 90, Name = Resources.Card_OlumpiaiZeuszSzobor, Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Brown = 7 }, VictoryPoints = 4 },
        };
    }
}
