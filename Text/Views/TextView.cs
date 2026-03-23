namespace Zion
{
    public abstract class TextView : ObjectView<char>
    {
        public abstract override string ToString();


        protected sealed override bool Equals(char A, char B)
        {
            return A == B;
        }


        public bool Begins(int Start, string Target, Func<char, char, bool>? Equals = null)
        {
            return Begins(Start, Target, Target.Length, Equals);
        }


        public string Substring(int Start, int Length)
        {
            if (Start < 0 || Start > this.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Start),
                    $"Start index {Start} is out of range [0, {this.Length}]");
            }

            if (Length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Length),
                    $"Length {Length} cannot be negative");
            }

            if (Start + Length > this.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Length),
                    $"Start + Length ({Start + Length}) exceeds the length of the text ({this.Length})");
            }

            if (Length == 0)
            {
                return string.Empty;
            }

            char[] Chars = new char[Length];
            for (int i = 0; i < Length; i++)
            {
                Chars[i] = this[Start + i];
            }

            return new string(Chars);
        }

        public string Substring(Range Range)
        {
            int Start = Range.Start.IsFromEnd ? Length - Range.Start.Value : Range.Start.Value;
            int End = Range.End.IsFromEnd ? Length - Range.End.Value : Range.End.Value;

            if (Start < 0 || Start > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Range), $"Start index {Start} is out of range");
            }

            return End < 0 || End > Length
                ? throw new ArgumentOutOfRangeException(nameof(Range), $"End index {End} is out of range")
                : Start > End
                ? throw new ArgumentException($"Start index {Start} must be less than or equal to end index {End}")
                : Substring(Start, End - Start);
        }

        public string Substring(int Start)
        {
            return Substring(Start, Length - Start);
        }
    }
}