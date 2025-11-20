namespace Zion.STP
{
    public readonly struct WhiteSpace : ITokenTemplate
    {
        public bool IsMatch(StringView String, int Start, out Token Block)
        {
            int Length = String.SkipSpaces(Start) - Start;
            Block = new WhiteSpaceBlock(String, Length);
            return Length > 0;
        }
    }
}