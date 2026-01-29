namespace Zion.STP
{
    public abstract class ColoredToken : Token
    {
        public required RGBColor Color { get; init; }

        public sealed override RGBColor TextColor => Color;
    }
}