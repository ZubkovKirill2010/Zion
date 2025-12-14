namespace Zion.STP
{
    public abstract class Template
    {
        public required RGBColor Color { get; init; }

        public abstract bool Read(StringView String, int Start, out Block Block);
    }
}