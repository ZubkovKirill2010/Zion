namespace Zion.STP
{
    public sealed class StringTextSource : TextSource
    {
        private readonly string Source;

        private int Index
        {
            get;
            set
            {
                if (value < Source.Length)
                {
                    _Current = Source[value];
                    _IsEnd = false;
                }
                else
                {
                    _IsEnd = true;
                }

                field = value;
            }
        }

        private char _Current;
        private bool _IsEnd;

        public override char Current => _Current;
        public override bool IsEnd => _IsEnd;


        public StringTextSource(string Source) : this(Source, 0) { }
        public StringTextSource(string Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start);

            this.Source = Source.NotNull();
            Index = Start;
        }


        public override TextSource BeginNew()
        {
            return new StringTextSource(Source, Index);
        }

        public override void MoveNext()
        {
            Index++;
        }
    }
}