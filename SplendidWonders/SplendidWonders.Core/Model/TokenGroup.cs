namespace SplendidWonders.Core.Model
{
    public record struct TokenGroup
    {
        public int Yellow { get; set; }
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Brown { get; set; }
        public int Red { get; set; }
        public int Polymath { get; set; }

        public static TokenGroup FromArray(int[] values)
        {
            if (values.Length != 6)
                throw new ArgumentException("Array must have exactly 6 elements.");

            return new TokenGroup
            {
                Yellow = values[0],
                Blue = values[1],
                Green = values[2],
                Brown = values[3],
                Red = values[4],
                Polymath = values[5]
            };
        }
    }
}