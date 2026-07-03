using System.Collections.Concurrent;

namespace Zion.Serialization.NSD
{
    public sealed class NSDFileReader : INSDReader
    {
        #region Stream
        private readonly string FileName;
        private readonly long   TotalLength;

        private bool Disposed;

        public int MaxStreamCount { get; private set; } = 4;//TODO HDD=1 SSD=~16

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
        public void ReadKeys()
        {
            DataPositions.Clear();

            long TotalLength = this.TotalLength;

            BinaryReader Reader = GetReader(0);//TODO
            Stream Stream = Reader.BaseStream;

            while (Stream.Position <= TotalLength)
            {
                string Key = Reader.ReadString();
                long Length = Reader.ReadUInt32();

                DataPositions[Key] = Stream.Position;

                Stream.Seek(Length, SeekOrigin.Current);
            }
        }

        public bool TryReadContainer<T>(string Key, out T Value) where T : INSDContainer<T>
        {
            if (DataPositions.TryGetValue(Key, out long Start))
            {
                //TODO: use isolation binary reader
                Value = T.Read();
                return true;
            }
            Value = default!;
            return false;
        }

        public bool TryReadPrimitive<T>(string Key, out T Value) where T : IBinaryReadable<T>
        {
            if (DataPositions.TryGetValue(Key, out long Start))
            {
                Value = T.Read(GetReader(Start));
                return true;
            }
            Value = default!;
            return false;
        }

        public bool TryRead<T>(string Key, out T Value, IBinaryReader<T>? ObjectReader = null)
        {
            ObjectReader ??= BinarySerializer.GetReader<T>();

            BinarySerializer.ReaderNotFound(ObjectReader);

            if (DataPositions.TryGetValue(Key, out long Start))
            {
                Value = ObjectReader.Read(GetReader(Start));
                return true;
            }
            Value = default!;
            return false;
        }

        #endregion

        #region Streams
        private BinaryReader GetReader(long Position)
        {
            //TODO: From ObjectPool<BinaryReader>
            return NewReader();
        }

        private BinaryReader NewReader()
        {
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

        #region PrivateMethods
        private async Task<bool> TryReadAsync<T>(string Key, Action<T> Setter, Func<BinaryReader, T> Read)
        {
            //TODO
            if (DataPositions.TryGetValue(Key, out long Start))
            {
                BinaryReader Reader = GetReader(Start);
                Setter(Read(Reader));
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }

        #endregion
    }
}