namespace Zion.STP
{
    public abstract class ColoredTokenTemplate : TokenTemplate
    {
        public required RGBColor Color { get; init; }
    }
}