namespace Zion.STP
{
    public sealed class EmptyToken : Token
    {
        public override int Length => 0;
        public override bool IsReadOnly => true;

        public EmptyToken()
        {
            Color = default;
        }

        public override bool Check(StringView String, int Start)
        {
            return true;
        }

        public override IEnumerable<ColorChar> Enumerate(StringView String, int Start)
        {
            yield break;
        }
    }
}