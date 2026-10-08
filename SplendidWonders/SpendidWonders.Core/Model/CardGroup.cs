namespace SplendidWonders.Core.Model
{
    public class CardGroup
    {
        public IReadOnlyList<DevelopmentCard> Level1 { get; set; }
        public IReadOnlyList<DevelopmentCard> Level2 { get; set; }
        public IReadOnlyList<DevelopmentCard> Level3 { get; set; }
    }
}