namespace Zion.Serialization.NSD
{
    public sealed class NSDReadContext : INSDReadProvider
    {
        private readonly INSDReadProvider Provider;

        public NSDReadContext(INSDReadProvider Provider)
        {
            this.Provider = Provider.NotNull();
        }

        public bool TryReadContainer<T>(string Key, out T Value) where T : INSDContainer<T>
        {
            return Provider.TryReadContainer(Key, out Value);
        }

        public bool TryReadPrimitive<T>(string Key, out T Value) where T : IBinarySerializable<T>
        {
            return Provider.TryReadPrimitive(Key, out Value);
        }

        public bool TryRead<T>(string Key, out T Value, IBinaryReader<T>? ObjectReader = null)
        {
            return Provider.TryRead(Key, out Value, ObjectReader);
        }
    }
}