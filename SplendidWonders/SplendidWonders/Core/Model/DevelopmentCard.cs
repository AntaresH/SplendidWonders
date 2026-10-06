using System.Drawing;

namespace SplendidWonders.Core.Model
{
    public class DevelopmentCard
    {
        public string Name { get; set; }

        public int VictoryPoints { get; set; }

        public TokenGroup Cost { get; set; }

        public BonusColor Bonus { get; set; }
    }
}