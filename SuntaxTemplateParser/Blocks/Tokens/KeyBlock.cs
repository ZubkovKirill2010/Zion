namespace Zion.STP
{
    public sealed class KeyBlock : Token
    {
        private readonly string Key;

        public KeyBlock(StringView String, string Key, ITokenTemplate Template)
            : base(String, Key.Length, Template)
        {
            this.Key = Key;
        }

        public override bool IsValid(int Start)
        {
            return String.Begins(Start, Key);
        }
    }
}