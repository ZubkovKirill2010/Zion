namespace Zion.STP
{
    public sealed class EmptyBlock : Token
    {
        public EmptyBlock(StringView String, ITokenTemplate Template)
            : base(String, 0, Template) { }

        public override bool IsValid(int Start)
        {
            return Length == 0;
        }
    }
}