namespace Zion.STP
{
    public readonly struct SingleCharTokenReader : ITokenReader
    {
        private readonly Dictionary<char, Func<char, Token>> Map;

        public SingleCharTokenReader(ICollection<KeyValuePair<char, Func<char, Token>>> Map)
        {
            this.Map = Map.ToDictionary();
        }

        public bool Read(ref TextSource Source, out Token Token)
        {
            char Char = Source.Current;

            if (Map.TryGetValue(Char, out Func<char, Token>? TokenCreater))
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