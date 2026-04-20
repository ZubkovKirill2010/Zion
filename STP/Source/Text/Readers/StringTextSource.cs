namespace Zion.STP
{
    public sealed class StringTextSource : ITextSource
    {
        private readonly string Source;

        private readonly int Start;

        private int Index
        {
            get;
            set
            {
                if (value < Source.Length)
                {
                    Current = Source[value];
                    IsEnd = false;
                }
                else
                {
                    IsEnd = true;
                }

                field = value;
            }
        }

        public char Current { get; private set; }
        public bool IsEnd { get; private set; }


        public StringTextSource(string Source) : this(Source, 0) { }
        public StringTextSource(string Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source);

            this.Source = Source.NotNull();
            this.Start = Start;
            Index = Start;
        }


        public ITextSource BeginNew()
        {
            return new StringTextSource(Source, Index);
        }

        public bool MoveNext()
        {
            if (IsEnd) { return false; }

            Index++;
            return !IsEnd;
        }

        public void Reset()
        {
            Index = Start;
        }
    }
}