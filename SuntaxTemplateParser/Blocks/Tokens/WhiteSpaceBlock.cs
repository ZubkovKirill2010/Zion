namespace Zion.STP
{
    public sealed class WhiteSpaceBlock : Token
    {
        public WhiteSpaceBlock(StringView String, int Length, ITokenTemplate Template)
            : base(String, Length, Template) { }

        public override bool IsValid(int Start)
        {
            return IsValid(Start, char.IsWhiteSpace);
        }
    }
}
