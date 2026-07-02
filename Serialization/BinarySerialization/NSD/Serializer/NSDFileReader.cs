using System.Collections.Concurrent;

namespace Zion.Serialization.NSD
{
    public sealed class NSDFileReader : IDisposable, IAsyncDisposable
    {
        #region Stream
        private readonly string FileName;
        private readonly long   TotalLength;

        private bool Disposed;

        public int MaxStreamCount { get; private set; } = 4;//TODO HDD=~4 SSD=~16

        #endregion

        #region Data
        private readonly ConcurrentDictionary<string, long> DataPositions = new();

        #endregion

        #region Constructors
        public NSDFileReader(string FilePath)
        {
            FileNotFoundException.ThrowIfNotExists(FilePath);
            FileName = FilePath;
            TotalLength = new FileInfo(FileName).Length;
        }

        #endregion

        #region Reading
        public async Task ReadKeysAsync()
        {
            DataPositions.Clear();

            long Current = 0L;
            long TotalLength = this.TotalLength;

            BinaryReader Reader = await GetReader();//TODO: From ObjectPool<BinaryReader>
            Stream Stream = Reader.BaseStream;

            while (Current <= TotalLength)
            {
                string Key = Reader.ReadString();
                long Length = Reader.ReadUInt32();

                DataPositions[Key] = Current;
                Current += Length;

                Stream.Seek(Length, SeekOrigin.Current);
            }
        }


        //TODO: NSDReader.Read...
        public T ReadSizable<T>(string Key) where T : INSDSizable<T>
        {
            throw new NotImplementedException();
        }

        public T ReadPrimitive<T>(string Key) where T : IBinarySerializable<T>
        {
            throw new NotImplementedException();
        }

        public T ReadContainer<T>(string Key) where T : INSDContainer<T>
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Streams
        private async Task<BinaryReader> GetReader()
        {
            //TODO: From ObjectPool<BinaryReader>
            return NewReader();
        }

        private BinaryReader NewReader()
        {
            //Stream.Position = 0!;
            return new BinaryReader
            (
                new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            );
        }

        #endregion

        #region Disposing
        public void Dispose()
        {
            if (Disposed) { return; }
            Disposed = true;
            //TODO: ObjectPool.Dispose
        }

        public async ValueTask DisposeAsync()
        {
            if (Disposed) { return; }
            Disposed = true;
            //TODO: ObjectPool.Dispose
        }

        #endregion
    }
}