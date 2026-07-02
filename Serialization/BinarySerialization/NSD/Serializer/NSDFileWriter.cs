namespace Zion.Serialization.NSD
{
    public sealed class NSDFileWriter : IDisposable, IAsyncDisposable, INSDWriteProvider
    {
        private readonly HashSet<string> UsingKeys = new();

        private readonly FileStream Stream;
        private readonly BinaryWriter Writer;
        private bool Disposed;

        public NSDFileWriter(FileStream Stream)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            ArgumentException.ThrowIf(!Stream.CanWrite, "");
            this.Stream = Stream;
            this.Writer = new BinaryWriter(Stream);
        }
        
        public NSDFileWriter(string FilePath)
        {
            FileNotFoundException.ThrowIfNotExists(FilePath);
            Stream = new FileStream(FilePath, FileMode.Open, FileAccess.Write, FileShare.None);
            Writer = new BinaryWriter(Stream);
        }


        public void AddSizable<T>(string Key, T Value) where T : INSDSizable<T>
        {
            CheckKeyAndValue(Key, Value);
            
            Writer.Write(Key);
            Writer.Write(Value.BinarySize);
            
            Value.Write(Writer);
        }

        public void AddPrimitive<T>(string Key, T Value) where T : INSDPrimitive<T>
        {
            CheckKeyAndValue(Key, Value);
            Write(Key, () => Value.Write(Writer));
        }

        public void AddContainer<T>(string Key, T Value) where T : INSDContainer<T>
        {
            CheckKeyAndValue(Key, Value);
            Write(Key, () => Value.Write(new NSDWriteContext(this)));
        }


        public void Dispose()
        {
            if (Disposed) { return; }
            Disposed = true;
            Stream.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (Disposed) { return; }
            Disposed = true;
            await Stream.DisposeAsync();
        }


        private void CheckKeyAndValue<T>(string Key, T Value)
        {
            ArgumentNullException.ThrowIfNull(Key);
            ArgumentNullException.ThrowIfNull(Value);
            ArgumentException.ThrowIf(!UsingKeys.Add(Key), $"Key '{Key}' already exists");
        }

        private void Write(string Key, Action Write)
        {
            Writer.Write(Key);

            long SizePosition = Stream.Position;
            Stream.Seek(4, SeekOrigin.Current);

            Write();

            long Position = Stream.Position;
            Stream.Position = SizePosition;
            Writer.Write((uint)(Position - SizePosition - 4));

            Stream.Position = Position;
        }
    }
}