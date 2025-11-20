namespace Zion.STP
{
    public readonly struct KeyWord : ITokenTemplate
    {
        public readonly string Key;
        public bool IgnoreCase { get; init; }

        public KeyWord(string KeyWord)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(KeyWord);
            Key = KeyWord;
        }

        public bool IsMatch(StringView String, int Start, out Token Block)
        {
            if (String.Begins(Start, Key, IgnoreCase))
            {
                Block = new KeyBlock(String, Key);
                return true;
            }
            Block = null;
            return false;
        }
    }
}