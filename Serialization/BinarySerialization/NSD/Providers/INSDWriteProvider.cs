namespace Zion.Serialization.NSD
{
    public interface INSDWriteProvider
    {
        public void AddContainer<T>(string Key, T Value) where T : INSDContainer<T>;

        public void AddPrimitive<T>(string Key, T Value) where T : IBinaryWritable;

        public void AddSizable<T>(string Key, T Value) where T : INSDSizable<T>;

        public void Add<T>(string Key, T Value, IBinaryWriter<T>? ObjectWriter = null);
    }
}