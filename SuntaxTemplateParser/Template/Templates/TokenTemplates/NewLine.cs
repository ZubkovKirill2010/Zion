namespace Zion.STP
{
    public readonly struct NewLine : ITokenTemplate
    {
        public RGBColor Color { get => default; init { } }

        public bool IsMatch(StringView String, int Start, out Block Block)
        {
            if (String.Begins(Start, "\r\n"))
            {
                Block = new NewLineBlock(String, 2, this);
                return true;
            }
            if (Start < String.Length && String[Start] == '\n')
            {
                Block = new NewLineBlock(String, 1, this);
                return true;
            }
            Block = default;
            return false;
        }
    }
}