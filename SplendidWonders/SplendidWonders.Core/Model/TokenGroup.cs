using SplendidWonders.Core.Setup;

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
            if (values.Length != 5 && values.Length != 6)
            {
                throw new ArgumentException("Array must have exactly 5 or 6 elements!");
            }

            var returnTokenGroup = new TokenGroup { Yellow = values[0], Blue = values[1], Green = values[2], Brown = values[3], Red = values[4] };

            if (values.Length == 6)
                returnTokenGroup.Polymath = values[5];

            return returnTokenGroup;
        }


        public int[] ToArray()
        {
            return [ Yellow, Blue, Green, Brown, Red, Polymath ];
        }
    }
}