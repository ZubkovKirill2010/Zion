namespace Zion.Serialization.NSD
{
    public sealed class NSDSeekableReadContext : NSDReadContext
    {
        public NSDSeekableReadContext(Stream Stream) : base(Stream)
        {
            ArgumentException.ThrowIf(!Stream.CanSeek, "The stream does not support seeking");
        }
    }
}