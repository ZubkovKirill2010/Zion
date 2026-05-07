namespace Zion.STP
{
    public readonly struct CharToken : IValueToken<char>
    {
        public int Length { get; init; }
        public char Value { get; init; }
        public TokenStatus Status { get; init; }

        public override string ToString() => $"['{Value}']";
    }

    public readonly struct CharTokenReader : ITokenReader
    {
        public bool Read(ref TextSource Source, out IToken Token)
        {
            //#Realize
            throw new NotImplementedException();
        }
    }
}