namespace Zion.STP
{
    public abstract class Token : Block
    {
        public RGBColor Color { get; init; } = RGBColor.White;

        public virtual bool IsReadOnly => false;

        public override IEnumerable<ColorChar> Enumerate(StringView String, int Start)
        {
            return String.Range(Start, Length).Select(Char => new ColorChar(Char, Color));
        }
    }
}