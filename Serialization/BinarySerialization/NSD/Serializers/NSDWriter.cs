namespace Zion.Serialization.NSD
{
    public class NSDWriter : IDisposable, IAsyncDisposable
    {
        private readonly Stream Stream;
        private readonly NSDWriteContext Context;

        public NSDWriter(Stream Stream)
        {
            this.Stream = Stream.NotNull();
            this.Context = Stream.CanSeek
                ? new NSDSequentialWriteContext(Stream)
                : new NSDParallelWriteContext(Stream);
        }


        public void WritePrimitive<T>(string Key, T Value) where T : IBinarySerializable<T>
        {
            Context.WritePrimitive(Key, Value);
        }

        public void Write<T>(string Key, T Value) where T : INSDContainer<T>
        {
            Context.Write(Key, Value);
        }

        public void Write<T>(string Key, T Value, IBinaryWriter<T>? ObjectWriter = null)
        {
            Context.Write(Key, Value, ObjectWriter);
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