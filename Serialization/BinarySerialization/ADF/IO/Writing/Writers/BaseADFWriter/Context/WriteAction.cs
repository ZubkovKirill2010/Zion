namespace Zion.Serialization.ADF
{
    internal delegate void WriteAction<T>(ArenaStream Stream, ReadOnlySpan<T> Span, bool Compression);
}