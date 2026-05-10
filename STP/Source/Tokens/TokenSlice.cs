using System.Collections;

namespace Zion.STP
{
    public readonly struct TokenSlice : IEnumerable<Token>
    {
        private readonly List<Token> Source;
        private readonly int Start;

        public readonly int Count;


        public TokenSlice(TokenSlice Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source.Count);

            this.Source = Source.Source;
            Count = Source.Count - Start;
            this.Start = Source.Start + Start;
        }

        public TokenSlice(List<Token> Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source);

            this.Source = Source.NotNull();
            Count = Source.Count - Start;
            this.Start = Start;
        }


        public Token this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
                return Source[Start + Index];
            }
        }


        public IEnumerable<Token> EnumerateFrom(int Start)
        {
            return Source.EnumerateFrom(this.Start + Start);
        }


        public IEnumerator<Token> GetEnumerator()
        {
            return Source.EnumerateFrom(Start).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}