namespace Zion.STP
{
    public readonly struct ErrorToken : IToken
    {
        public int Length { get; init; }
        public TokenStatus Status { get; init; } = TokenStatus.Valid;

        public ErrorToken() { }

        public override string ToString() => "[Error]";
    }
}