namespace Zion.STP
{
    public sealed class KeyWordTemplate : TokenTemplate
    {
        private readonly string Key;
        private readonly bool IgnoreCase;

        public KeyWordTemplate(string Key, bool IgnoreCase = false)
        {
            this.Key = Key;
            this.IgnoreCase = IgnoreCase;
        }

        public override bool Read(StringView String, int Start, out Token Token)
        {
            return Accessor.Try
            (
                String.Begins(Start, Key, IgnoreCase),
                () => new KeyWordToken(Key, IgnoreCase) { Color = Color },
                out Token
            );
        }
    }
}