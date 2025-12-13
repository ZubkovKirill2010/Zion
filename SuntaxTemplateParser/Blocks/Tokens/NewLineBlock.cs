namespace Zion.STP
{
    public sealed class NewLineBlock : Token
    {
        public NewLineBlock(StringView String, int Length, ITokenTemplate Template)
            : base(String, Length, Template) { }

        public override bool IsValid(int Start)
        {
            return String.Begins(Start, "\r\n") || (Start < String.Length && String[Start] == '\n');
        }
    }
}