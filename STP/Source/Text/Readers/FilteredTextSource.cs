namespace Zion.STP
{
    public sealed class FilteredTextSource : ITextSource
    {
        private readonly ITextSource Source;
        private readonly Func<char, bool> Filter;

        public char Current => Source.Current;
        public bool IsEnd => Source.IsEnd;


        public FilteredTextSource(ITextSource Source, Func<char, bool> Filter)
        {
            this.Source = Source.NotNull();
            this.Filter = Filter.NotNull();

            Update();
        }


        public ITextSource BeginNew()
        {
            return new FilteredTextSource(Source.BeginNew(), Filter);
        }

        public bool MoveNext()
        {
            do
            {
                if (IsEnd) { return false; }
                Source.MoveNext();
            }
            while (!Filter(Current));

            return true;
        }

        public void Reset()
        {
            Source.Reset();
            Update();
        }

        private void Update()
        {
            if (!IsEnd && !Filter(Current))
            {
                MoveNext();
            }
        }
    }
}