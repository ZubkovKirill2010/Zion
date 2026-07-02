namespace Zion.STP
{
    public readonly struct IdentifierTokenReader : ITokenReader
    {
        public bool AllowDigits { get; init; } = true;

        public IdentifierTokenReader() { }

        public bool Read(ref TextSource Source, out Token Token)
        {
            Token = default!;
            List<char> Chars = new List<char>(20);

            if (Source.CurrentIs(Char => Char == '_' || Translater.IsEnglish(Char)))
            {
                Chars.Add(Source.Current);
                Source.MoveNext();
            }
            else
            {
                return false;
            }

            while (!Source.IsEnd)
            {
                char Current = Source.Current;

                if (Current == '_' || Translater.IsEnglish(Current) || (AllowDigits && Current.IsDigit()))
                {
                    Chars.Add(Current);
                    Source.MoveNext();
                    continue;
                }

                return false;
            }

            if (Chars.Count == 0)
            {
                return false;
            }

            Token = new WordToken
            {
                Length = Chars.Count,
                Value = Text.Create(Chars)
            };
            return true;
        }
    }
}