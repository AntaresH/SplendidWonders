using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public static class Cards
    {
        public static readonly IReadOnlyList<DevelopmentCard> Level1 = new List<DevelopmentCard>
        {
            new DevelopmentCard { Id = 1, Name = "Bíróság", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 2, Green = 2, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 2, Name = "Borászat", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 3, Name = "Kereskedőház", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 3, Blue = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 4, Name = "Kikötő", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Brown = 2, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 5, Name = "Magtár", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 1, Green = 2, Brown = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 6, Name = "Piac", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 1, Green = 1, Brown = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 7, Name = "Tanács", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 2, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 8, Name = "Városháza", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Green = 4 }, VictoryPoints = 0 },

            new DevelopmentCard { Id = 9, Name = "Cirkusz", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Red = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 10, Name = "Fürdő", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 1, Green = 3, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 11, Name = "Kertek", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Green = 2, Brown = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 12, Name = "Palota", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Brown = 4 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 13, Name = "Sírkamra", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 14, Name = "Szentély", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Green = 2, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 15, Name = "Színház", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Green = 1, Brown = 2, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 16, Name = "Templom", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 1, Green = 1, Brown = 1, Red = 1 }, VictoryPoints = 0 },

            new DevelopmentCard { Id = 17, Name = "Iskola", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 1, Blue = 1, Brown = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 18, Name = "Kórház", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Red = 4 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 19, Name = "Könyvtár", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 1, Blue = 3, Green = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 20, Name = "Laboratórium", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 1, Blue = 1, Brown = 1, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 21, Name = "Levéltár", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 2, Blue = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 22, Name = "Papíruszgyár", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 2, Brown = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 23, Name = "Patika", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 1, Brown = 2, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 24, Name = "Üveggyár", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Brown = 3 }, VictoryPoints = 0 },

            new DevelopmentCard { Id = 25, Name = "Ácsműhely", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Green = 1, Red = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 26, Name = "Bánya", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 27, Name = "Fűrésztelep", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Blue = 2, Green = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 28, Name = "Hajógyár", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Brown = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 29, Name = "Kőfejtő", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 1, Blue = 1, Green = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 30, Name = "Öntöde", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 1, Brown = 1, Red = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 31, Name = "Téglagyár", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Blue = 1, Green = 1, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 32, Name = "Vízvezeték", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 4 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 33, Name = "Aréna", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Blue = 4 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 34, Name = "Erőd", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 1, Brown = 3, Red = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 35, Name = "Fegyverkovács", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 2, Blue = 2, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 36, Name = "Helyőrség", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 2, Green = 2 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 37, Name = "Istálló", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 3 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 38, Name = "Laktanya", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 2, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 39, Name = "Őrtorony", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 1, Blue = 1, Green = 1, Brown = 1 }, VictoryPoints = 0 },
            new DevelopmentCard { Id = 40, Name = "Városfal", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 1, Blue = 2, Green = 1, Brown = 1 }, VictoryPoints = 0 },
        };

        public static readonly IReadOnlyList<DevelopmentCard> Level2 = new List<DevelopmentCard>
        {
            new DevelopmentCard { Id = 41, Name = "Államigazgatás", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Brown = 5, Red = 3 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 42, Name = "Köztársaság", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 43, Name = "Monarchia", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Green = 1, Brown = 4, Red = 2 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 44, Name = "Pénzverés", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Brown = 5}, VictoryPoints = 2 },
            new DevelopmentCard { Id = 45, Name = "Tengerészet", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Green = 3, Brown = 2, Red = 2 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 46, Name = "Törvénykönyv", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 2, Blue = 3, Brown = 3 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 47, Name = "Filozófia", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 48, Name = "Iparművészet", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 2, Green = 3, Brown = 3}, VictoryPoints = 1 },
            new DevelopmentCard { Id = 49, Name = "Irodalom", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 50, Name = "Képzőművészet", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 5, Blue = 3}, VictoryPoints = 2 },
            new DevelopmentCard { Id = 51, Name = "Misztika", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Blue = 2, Green = 3, Red = 3 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 52, Name = "Zene", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 2, Brown = 1, Red = 4 }, VictoryPoints = 2 },

            new DevelopmentCard { Id = 53, Name = "Alkímia", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Green = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 54, Name = "Ábécé", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 5, Green = 3 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 55, Name = "Csillagászat", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 4, Blue = 2, Red = 1 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 56, Name = "Matematika", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Green = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 57, Name = "Naptár", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 3, Green = 2, Brown = 3 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 58, Name = "Térképészet", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 2, Blue = 3, Red = 2 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 59, Name = "Építészet", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 1, Blue = 4, Green = 2 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 60, Name = "Hídépítés", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 3, Red = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 61, Name = "Mezőgazdaság", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 2, Green = 2, Red = 3 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 62, Name = "Mérnökség", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Red = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 63, Name = "Útrendszer", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Red = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 64, Name = "Várostervezés", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Blue = 3, Brown = 2, Red = 3 }, VictoryPoints = 1 },

            new DevelopmentCard { Id = 65, Name = "Armada", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 5 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 66, Name = "Harci Szekerek", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 3, Blue = 2, Green = 2 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 67, Name = "Lovas Íjászat", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 3, Green = 3, Red = 2 }, VictoryPoints = 1 },
            new DevelopmentCard { Id = 68, Name = "Ostromszakértelem", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 5, Brown = 3 }, VictoryPoints = 2 },
            new DevelopmentCard { Id = 69, Name = "Hadvezetés", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Red = 6 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 70, Name = "Vasművesség", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Blue = 1, Green = 4, Brown = 2 }, VictoryPoints = 2 },
        };

        public static IReadOnlyList<DevelopmentCard> Level3 = new List<DevelopmentCard>
        {
            new DevelopmentCard { Id = 71, Name = "Alexandriai Világítótorony", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 3, Brown = 3, Red = 6 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 72, Name = "Perszepolisz", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Yellow = 3, Red = 7 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 73, Name = "Petra", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Blue = 2, Green = 3, Brown = 5, Red = 3 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 74, Name = "Rodoszi Kolosszus", Bonus = DevelopmentBonus.Yellow, Cost = new TokenGroup { Red = 7 }, VictoryPoints = 4 },

            new DevelopmentCard { Id = 75, Name = "Agyaghadsereg", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 3, Green = 2, Brown = 3, Red = 5 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 76, Name = "Epheszoszi Artemisz Templom", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 7, Blue = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 77, Name = "Halikarnasszoszi Mauzóleum", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 7 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 78, Name = "Knósszoszi Palota", Bonus = DevelopmentBonus.Blue, Cost = new TokenGroup { Yellow = 6, Blue = 3, Red = 3 }, VictoryPoints = 4 },

            new DevelopmentCard { Id = 79, Name = "Alexandriai Nagy Könyvtár", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 7, Green = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 80, Name = "Gundésápur Egyetem", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 3, Blue = 6, Green = 3 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 81, Name = "Nalanda Kolostor", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Blue = 7 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 82, Name = "Uri Nagy Ziggurat", Bonus = DevelopmentBonus.Green, Cost = new TokenGroup { Yellow = 5, Blue = 3, Brown = 2, Red = 3 }, VictoryPoints = 3 },

            new DevelopmentCard { Id = 83, Name = "Babiloni Függőkertek", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Green = 7 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 84, Name = "Gízai Piramisok", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Green = 7, Brown = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 85, Name = "Hagia Sophia", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Blue = 3, Green = 6, Brown = 3 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 86, Name = "Pantheon", Bonus = DevelopmentBonus.Brown, Cost = new TokenGroup { Yellow = 3, Blue = 5, Green = 3, Red = 2 }, VictoryPoints = 3 },

            new DevelopmentCard { Id = 87, Name = "Istár Kapuja", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Yellow = 2, Blue = 3, Green = 5, Brown = 3 }, VictoryPoints = 3 },
            new DevelopmentCard { Id = 88, Name = "Kínai Nagy Fal", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Brown = 7, Red = 3 }, VictoryPoints = 5 },
            new DevelopmentCard { Id = 89, Name = "Kolosszeum", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Green = 3, Brown = 6, Red = 3 }, VictoryPoints = 4 },
            new DevelopmentCard { Id = 90, Name = "Olümpiai Zeusz Szobor", Bonus = DevelopmentBonus.Red, Cost = new TokenGroup { Brown = 7 }, VictoryPoints = 4 },
        };
    }
}
