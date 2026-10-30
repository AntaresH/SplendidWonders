using System.Drawing;

namespace SplendidWonders.Core.Model
{
    public record DevelopmentCard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level {  get { return Id < 41 ? 1 : Id < 71 ? 2 : 3; } }

        public int VictoryPoints { get; set; }

        public TokenGroup Cost { get; set; }

        public DevelopmentBonus Bonus { get; set; }
    }
}