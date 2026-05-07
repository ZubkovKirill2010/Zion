namespace Zion.STP
{
    public readonly record struct NewLineToken : IToken
    {
        public int Length { get; init; }
        public TokenStatus Status { get; init; }

        public override string ToString() => "[\\n]";
    }

    public readonly struct NewLineReader : ITokenReader
    {
        public bool Read(ref TextSource Source, out IToken Token)
        {
            if (Source.Begins("\n"))
            {
                Token = new NewLineToken() { Length = 1 };
                return true;
            }
            else if (Source.Begins("\r\n"))
            {
                Token = new NewLineToken() { Length = 1 };
                return true;
            }

            Token = null!;
            return false;
        }
    }
}