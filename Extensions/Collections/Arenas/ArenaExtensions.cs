namespace Zion
{
    public static class ArenaExtensions
    {
        extension(Arena<byte> Arena)
        {
            public ArenaStream GetStream(int Size)
            {
                return Arena.Allocate
                (
                    Arena<byte>.RoundToGroup(Size),
                    static Span => new ArenaStream(Span)
                );
            }
        }
    }
}