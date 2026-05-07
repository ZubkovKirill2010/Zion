namespace Zion.STP
{
    public readonly struct SingleCharTokenReader : ITokenReader
    {
        private readonly Dictionary<char, Func<char, IToken>> Map;

        public SingleCharTokenReader(ICollection<KeyValuePair<char, Func<char, IToken>>> Map)
        {
            this.Map = Map.ToDictionary();
        }

        public bool Read(ref TextSource Source, out IToken Token)
        {
            char Char = Source.Current;

            if (Map.TryGetValue(Char, out Func<char, IToken>? TokenCreater))
            {
                Source.MoveNext();

                Token = TokenCreater(Char);
                return true;
            }

            Token = default!;
            return false;
        }
    }
}