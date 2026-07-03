namespace Zion.Serialization.NSD
{
    public sealed class NSDBufferedReadContext : NSDReadContext
    {
        public NSDBufferedReadContext(Stream Stream) : base(Stream) { }
    }
}