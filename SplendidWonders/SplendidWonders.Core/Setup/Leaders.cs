using SplendidWonders.Core.Model;
using SplendidWonders.Core.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public class Leaders
    {
        public static IReadOnlyList<Leader> StandardLeaders = new List<Leader>()
        {
            new Leader() { Name = Resources.Leader_Salamon, Requirements = new TokenGroup() { Yellow = 4, Blue = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Periklesz, Requirements = new TokenGroup() { Blue = 4, Green = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Nabukodonozor, Requirements = new TokenGroup() { Green = 4, Brown = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_JuliusCaesar, Requirements = new TokenGroup() { Brown = 4, Red = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_NagyKurosz, Requirements = new TokenGroup() { Red = 4, Yellow = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Amenhotep, Requirements = new TokenGroup() { Yellow = 3, Blue = 3, Green = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Ptolemaiosz, Requirements = new TokenGroup() { Blue = 3, Green = 3, Brown = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_CsinSiHuangTi, Requirements = new TokenGroup() { Green = 3, Brown = 3, Red = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Hammurapi, Requirements = new TokenGroup() { Brown = 3, Red = 3, Yellow = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_AugustusCaesar, Requirements = new TokenGroup() { Red = 3, Yellow = 3, Blue = 3 }, VictoryPoints = 3 },
        };
        public static IReadOnlyList<Leader> ExtendedLeaders = new List<Leader>()
        {
            new Leader() { Name = Resources.Leader_Csandragupta, Requirements = new TokenGroup() { Yellow = 4, Green = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Ramszesz, Requirements = new TokenGroup() { Blue = 4, Brown = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Asszurbanipal, Requirements = new TokenGroup() { Green = 4, Red = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Darius, Requirements = new TokenGroup() { Brown = 4, Yellow = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_NagySandor, Requirements = new TokenGroup() { Red = 4, Blue = 4 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_HanVuTi, Requirements = new TokenGroup() { Yellow = 3, Blue = 3, Brown = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Szuppiluliumasz, Requirements = new TokenGroup() { Blue = 3, Green = 3, Red = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Hiram, Requirements = new TokenGroup() { Green = 3, Brown = 3, Yellow = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Sarrukin, Requirements = new TokenGroup() { Brown = 3, Red = 3, Blue = 3 }, VictoryPoints = 3 },
            new Leader() { Name = Resources.Leader_Hannibal, Requirements = new TokenGroup() { Red = 3, Yellow = 3, Green = 3 }, VictoryPoints = 3 },
        };
    }
}
