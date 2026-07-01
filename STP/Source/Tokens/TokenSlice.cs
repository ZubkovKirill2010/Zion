using System.Collections;

namespace Zion.STP
{
    public readonly struct TokenSlice : IEnumerable<Token>
    {
        private readonly ListView<Token> Source;
        private readonly int Start;

        public readonly int Count;


        public TokenSlice(ListView<Token> Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source.Count);

            this.Source = Source;
            this.Count = Source.Count - Start;
            this.Start = Start;
        }

        public TokenSlice(ListView<Token> Source, int Start, int Count)
            : this(Source, Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Count, this.Count);

            this.Count = Count;
        }

        public TokenSlice(TokenSlice Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source.Count);

            this.Source = Source.Source;
            this.Count = Source.Count - Start;
            this.Start = Source.Start + Start;
        }

        public TokenSlice(TokenSlice Source, int Start, int Count)
            : this(Source, Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Count, this.Count);

            this.Count = Count;
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
            for (int i = this.Start + Start; i < Count; i++)
            {
                yield return Source[i];
            }
        }


        public IEnumerator<Token> GetEnumerator()
        {
            for (int i = Start; i < Count; i++)
            {
                yield return Source[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}