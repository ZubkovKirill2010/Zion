namespace Zion.Serialization.NSD
{
    public sealed class NSDParallelReadContext : NSDReadContext
    {
        private readonly Func<Stream> NewStream;
        private readonly int StreamLimit;

        public NSDParallelReadContext(Stream Stream, Func<Stream> NewStream, int StreamLimit = 1)
            : base(Stream)
        {
            ArgumentException.ThrowIf(!Stream.CanSeek, "The stream does not support seeking");
            ArgumentNullException.ThrowIfNull(NewStream);
            this.StreamLimit = Math.Max(StreamLimit, 1);
            this.NewStream = () =>
            {
                Stream Result = NewStream();
                return (Result.CanRead && Result.CanSeek)
                    ? Result
                    : throw new ArgumentException("New Stream can not Read or Seek");
            };
        }

        internal protected override void ReadAll(NSDBatchReader Batch)
        {
            Stream Stream = this.Stream;
            BinaryReader Reader = new BinaryReader(Stream);

            long TotalLength = Stream.Length;
            long Position = Stream.Position;

            List<Task> Tasks = new List<Task>();
            //TODO: Replace to Concurrent
            ObjectPool<Stream> StreamPool = new ObjectPool<Stream>
            (
                NewStream, null, null, StreamLimit
            );

            while (Stream.Position < TotalLength)
            {
                string Key = Reader.ReadString();
                long Size = Reader.ReadUInt32();

                if (Batch.TryGetSetter(Key, out Action<Stream> Setter))
                {
                    Stream LocalStream = StreamPool.Get();
                    LocalStream.Position = Stream.Position;

                    Tasks.Add
                    (
                        Task.Run
                        (
                            () =>
                            {
                                Setter(LocalStream);
                                StreamPool.Return(LocalStream);
                            }
                        )
                    );
                }

                Stream.Position += Size;
            }

            Task.WhenAll(Tasks).GetAwaiter().GetResult();
        }
    }
}