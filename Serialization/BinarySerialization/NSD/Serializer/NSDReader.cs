namespace Zion.Serialization.NSD
{
    public class NSDReader : IDisposable, IAsyncDisposable
    {
        private readonly Stream Stream;
        private readonly NSDReadContext Context;

        public NSDReader(Stream Stream)
        {
            this.Stream = Stream.NotNull();
            this.Context = Stream.CanSeek
                ? new NSDSeekableReadContext(Stream)
                : new NSDBufferedReadContext(Stream);
        }




        public void Dispose()
        {
            Stream.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return Stream.DisposeAsync();
        }
    }
}