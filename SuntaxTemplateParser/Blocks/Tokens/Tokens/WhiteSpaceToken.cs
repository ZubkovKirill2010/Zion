namespace Zion.STP
{
    public sealed class WhiteSpaceToken : Token
    {
        private int _Length;
        public override int Length => _Length;

        public WhiteSpaceToken(int Length)
        {
            _Length = Length;
        }

        public override bool Check(StringView String, int Start)
        {
            return Enumerable.All(String.Range(Start, Length), char.IsWhiteSpace);
        }
    }
}