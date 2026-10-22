using SplendidWonders.Common;
using SplendidWonders.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public class Leaders
    {
        public static IReadOnlyList<Leader> StandardLeaders = new List<Leader>()
        {
            new Leader() { Id = 1, Name = Strings.Leader_Darius, Requirements = new TokenGroup() { Yellow = 4, Blue = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 2, Name = Strings.Leader_Periklesz, Requirements = new TokenGroup() { Blue = 4, Green = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 3, Name = Strings.Leader_Amenhotep, Requirements = new TokenGroup() { Green = 4, Brown = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 4, Name = Strings.Leader_JuliusCaesar, Requirements = new TokenGroup() { Brown = 4, Red = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 5, Name = Strings.Leader_NagySandor, Requirements = new TokenGroup() { Red = 4, Yellow = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 6, Name = Strings.Leader_AugustusCaesar, Requirements = new TokenGroup() { Yellow = 3, Blue = 3, Green = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 7, Name = Strings.Leader_Asoka, Requirements = new TokenGroup() { Blue = 3, Green = 3, Brown = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 8, Name = Strings.Leader_CsinSiHuangTi, Requirements = new TokenGroup() { Green = 3, Brown = 3, Red = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 9, Name = Strings.Leader_NagyKurosz, Requirements = new TokenGroup() { Brown = 3, Red = 3, Yellow = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 10, Name = Strings.Leader_Hammurapi, Requirements = new TokenGroup() { Red = 3, Yellow = 3, Blue = 3 }, VictoryPoints = 3 },
        };
        public static IReadOnlyList<Leader> ExtendedLeaders = new List<Leader>()
        {
            new Leader() { Id = 11, Name = Strings.Leader_HanVuTi, Requirements = new TokenGroup() { Yellow = 4, Green = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 12, Name = Strings.Leader_Nabukodonozor, Requirements = new TokenGroup() { Blue = 4, Brown = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 13, Name = Strings.Leader_Asszurbanipal, Requirements = new TokenGroup() { Green = 4, Red = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 14, Name = Strings.Leader_Huszrau, Requirements = new TokenGroup() { Brown = 4, Yellow = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 15, Name = Strings.Leader_Sarrukin, Requirements = new TokenGroup() { Red = 4, Blue = 4 }, VictoryPoints = 3 },
            new Leader() { Id = 16, Name = Strings.Leader_Salamon, Requirements = new TokenGroup() { Yellow = 3, Blue = 3, Brown = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 17, Name = Strings.Leader_Szuppiluliumasz, Requirements = new TokenGroup() { Blue = 3, Green = 3, Red = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 18, Name = Strings.Leader_Ptolemaiosz, Requirements = new TokenGroup() { Green = 3, Brown = 3, Yellow = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 19, Name = Strings.Leader_Jusztinianusz, Requirements = new TokenGroup() { Brown = 3, Red = 3, Blue = 3 }, VictoryPoints = 3 },
            new Leader() { Id = 20, Name = Strings.Leader_Csandragupta, Requirements = new TokenGroup() { Red = 3, Yellow = 3, Green = 3 }, VictoryPoints = 3 },
        };
    }
}
