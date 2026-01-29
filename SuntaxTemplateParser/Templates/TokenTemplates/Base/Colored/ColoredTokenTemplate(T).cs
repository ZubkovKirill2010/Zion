namespace Zion.STP
{
    public abstract class ColoredTokenTemplate<T> : TokenTemplate<T>
    {
        public required RGBColor Color { get; init; }
    }
}