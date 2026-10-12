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
            new Leader() { Name = "Salamon", Requirements = new TokenGroup() { Yellow = 4, Blue = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Periklész", Requirements = new TokenGroup() { Blue = 4, Green = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Nabukodonozor", Requirements = new TokenGroup() { Green = 4, Brown = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Julius Caesar", Requirements = new TokenGroup() { Brown = 4, Red = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Nagy Kürosz", Requirements = new TokenGroup() { Red = 4, Yellow = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Amenhotep", Requirements = new TokenGroup() { Yellow = 3, Blue = 3, Green = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Ptolemaiosz", Requirements = new TokenGroup() { Blue = 3, Green = 3, Brown = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Csin Si Huang Ti", Requirements = new TokenGroup() { Green = 3, Brown = 3, Red = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Hammurapi", Requirements = new TokenGroup() { Brown = 3, Red = 3, Yellow = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Augustus Caesar", Requirements = new TokenGroup() { Red = 3, Yellow = 3, Blue = 3 }, VictoryPoints = 3 },
        };
        public static IReadOnlyList<Leader> ExtendedLeaders = new List<Leader>()
        {
            new Leader() { Name = "Csandragupta", Requirements = new TokenGroup() { Yellow = 4, Green = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Ramszesz", Requirements = new TokenGroup() { Blue = 4, Brown = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Asszurbanipál", Requirements = new TokenGroup() { Green = 4, Red = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Dárius", Requirements = new TokenGroup() { Brown = 4, Yellow = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Nagy Sándor", Requirements = new TokenGroup() { Red = 4, Blue = 4 }, VictoryPoints = 3 },
            new Leader() { Name = "Han Vu Ti", Requirements = new TokenGroup() { Yellow = 3, Blue = 3, Brown = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Szuppiluliumasz", Requirements = new TokenGroup() { Blue = 3, Green = 3, Red = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Hirám", Requirements = new TokenGroup() { Green = 3, Brown = 3, Yellow = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Sarrukín", Requirements = new TokenGroup() { Brown = 3, Red = 3, Blue = 3 }, VictoryPoints = 3 },
            new Leader() { Name = "Hannibál", Requirements = new TokenGroup() { Red = 3, Yellow = 3, Green = 3 }, VictoryPoints = 3 },
        };
    }
}
