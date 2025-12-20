namespace Zion.STP
{
    public abstract class TokenTemplate : Template
    {
        public required RGBColor Color { get; init; }

        public override sealed bool Read(StringView String, int Start, out Block Block)
        {
            bool Result = Read(String: String, Start: Start, Token: out Token Token);
            Block = Token;
            return Result;
        }

        public abstract bool Read(StringView String, int Start, out Token Token);
    }
}