namespace Zion
{
    public abstract class Parser<T>
    {
        public string String { get; private set; }

        public Parser(string String)
        {
            this.String = String;
        }

        public abstract T Parse();
        public virtual bool TryParse(out T? Value)
        {
            try
            {
                Value = Parse();
                return true;
            }
            catch
            {
                Value = default;
                return false;
            }
        }
    }

    public abstract class StreamingParser<T> : Parser<T>
    {
        protected int Index;
        protected int Start;

        protected StreamingParser(string String)
            : base(String) { }


        protected void Next() => Index++;

        protected void SkipSpaces()
        {
            String.Skip(ref Index, char.IsWhiteSpace);
        }
    }
}