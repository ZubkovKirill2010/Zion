namespace Zion.STP
{
    public sealed class KeyWordToken : ColoredToken
    {
        private readonly string Key;
        private readonly bool IgnoreCase;

        public override int Length => Key.Length;
        public override bool IsReadOnly => true;

        public KeyWordToken(string Key, bool IgnoreCase = false)
        {
            this.Key = Key;
            this.IgnoreCase = IgnoreCase;
        }

        public override bool Check(StringView String, int Start)
        {
            return String.Begins(Start, Key, IgnoreCase);
        }
    }
}