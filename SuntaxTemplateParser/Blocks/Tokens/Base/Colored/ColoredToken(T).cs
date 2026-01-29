namespace Zion.STP
{
    public abstract class ColoredToken<T> : Token<T>
    {
        public required RGBColor Color { get; init; }

        public sealed override RGBColor TextColor => Color;
    }
}