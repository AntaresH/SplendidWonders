using System.Drawing;

namespace SplendidWonders.Core.Model
{
    public class DevelopmentCard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int VictoryPoints { get; set; }

        public TokenGroup Cost { get; set; }

        public DevelopmentBonus Bonus { get; set; }
    }
}