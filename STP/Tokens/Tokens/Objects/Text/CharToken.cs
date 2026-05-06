namespace Zion.STP
{
    public readonly struct CharToken : IValueToken<char>
    {
        public int Length { get; init; }
        public char Value { get; init; }
    }

    public readonly struct CharTokenReader : ITokenReader
    {
        public bool Read(ref ITextSource Source, out IToken Token)
        {
            //#Realize
            throw new NotImplementedException();
        }
    }
}