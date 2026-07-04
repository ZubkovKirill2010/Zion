namespace Zion.Serialization.NSD
{
    public sealed class NSDBatchReader : IDisposable
    {
        private readonly NSDReadContext Context;
        private readonly NSDReadHandler Handler;

        public NSDBatchReader(NSDReadContext Context)
        {
            this.Context = Context.NotNull();
            this.Handler = new();
        }


        public bool ReadPrimitive<T>(string Key, Action<T> Setter) where T : IBinarySerializable<T>
        {
            return Handler.AddPrimitive(Key, Setter);
        }

        public bool ReadContainer<T>(string Key, Action<T> Setter) where T : INSDContainer<T>
        {
            return Handler.AddContainer(Key, Setter);
        }

        public bool Read<T>(string Key, Action<T> Setter, IBinaryReader<T>? ObjectReader = null)
        {
            return Handler.Add(Key, Setter, ObjectReader);
        }


        public bool TryRead(string Key, Stream Stream)
        {
            return Handler.TryRead(Key, Stream);
        }

        public bool TryGetSetter(string Key, out Action<Stream> Setter)
        {
            return Handler.TryGetSetter(Key, out Setter);
        }


        public void Dispose()
        {
            Context.ReadAll(this);
        }
    }
}