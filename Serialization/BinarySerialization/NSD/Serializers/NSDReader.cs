namespace Zion.Serialization.NSD
{
    public class NSDReader : IDisposable, IAsyncDisposable
    {
        private readonly NSDReadContext Context;
        private readonly Stream Stream;

        private readonly NSDBatchReader Batch;

        public NSDReader(Stream Stream, Func<Stream>? NewStream = null)
        {
            this.Stream = Stream.NotNull();
            this.Context = GetContext(Stream, NewStream);
            this.Batch = Context.BeginRead();
        }


        public bool ReadPrimitive<T>(string Key, Action<T> Setter) where T : IBinarySerializable<T>
        {
            return Batch.ReadPrimitive(Key, Setter);
        }

        public bool ReadContainer<T>(string Key, Action<T> Setter) where T : INSDContainer<T>
        {
            return Batch.ReadContainer(Key, Setter);
        }

        public bool Read<T>(string Key, Action<T> Setter, IBinaryReader<T>? ObjectReader = null)
        {
            return Batch.Read(Key, Setter, ObjectReader);
        }


        public void Dispose()
        {
            Batch.Dispose();
            Stream.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            Batch.Dispose();
            return Stream.DisposeAsync();
        }


        private static NSDReadContext GetContext(Stream Stream, Func<Stream>? NewStream)
        {
            return Stream.CanSeek && NewStream is not null
                ? new NSDParallelReadContext(Stream, NewStream)
                : new NSDSequentialReadContext(Stream);
        }
    }
}