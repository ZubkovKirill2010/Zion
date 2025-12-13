namespace Zion.STP
{
    public readonly struct WhiteSpace : ITokenTemplate
    {
        public RGBColor Color { get; init; }

        public bool IsMatch(StringView String, int Start, out Block Block)
        {
            int Length = String.SkipSpaces(Start) - Start;
            Block = new WhiteSpaceBlock(String, Length, this);
            return Length > 0;
        }
    }
}