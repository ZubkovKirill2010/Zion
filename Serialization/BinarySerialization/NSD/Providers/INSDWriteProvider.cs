namespace Zion.Serialization.NSD
{
    public interface INSDWriteProvider
    {
        public void AddContainer<T>(string Key, T Value) where T : INSDContainer<T>;

        public void AddPrimitive<T>(string Key, T Value) where T : INSDPrimitive<T>;

        public void AddSizable<T>(string Key, T Value) where T : INSDSizable<T>;
    }
}