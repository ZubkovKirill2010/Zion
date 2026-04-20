using System.Collections;

namespace Zion.STP
{
    public readonly struct TokenSlice : IEnumerable<IToken>
    {
        private readonly List<IToken> Source;
        private readonly int Start;

        public readonly int Count;


        public TokenSlice(TokenSlice Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source.Count);

            this.Source = Source.Source;
            Count = Source.Count - Start;
            this.Start = Source.Start + Start;
        }

        public TokenSlice(List<IToken> Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source);

            this.Source = Source.NotNull();
            Count = Source.Count - Start;
            this.Start = Start;
        }


        public IToken this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
                return Source[Start + Index];
            }
        }


        public IEnumerable<IToken> EnumerateFrom(int Start)
        {
            return Source.EnumerateFrom(this.Start + Start);
        }


        public IEnumerator<IToken> GetEnumerator()
        {
            return Source.EnumerateFrom(Start).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}