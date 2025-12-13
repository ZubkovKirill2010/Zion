namespace Zion.STP
{
    public readonly struct KeyWord : ITokenTemplate
    {
        public readonly string Key;
        public bool IgnoreCase { get; init; }

        public RGBColor Color { get; init; }

        public KeyWord(string KeyWord)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(KeyWord);
            Key = KeyWord;
        }

        public bool IsMatch(StringView String, int Start, out Block Block)
        {
            if (String.Begins(Start, Key, IgnoreCase))
            {
                Block = new KeyBlock(String, Key, this);
                return true;
            }
            Block = null;
            return false;
        }
    }
}