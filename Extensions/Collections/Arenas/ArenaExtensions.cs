namespace Zion
{
    public static class ArenaExtensions
    {
        extension(Arena<byte> Arena)
        {
            public ArenaStream GetStream(int Size)
            {
                return new ArenaStream(Arena.Allocate(Size));
            }
        }
    }
}