namespace Zion.Serialization.NSD
{
    public interface INSDSizable<T> : IBinarySerializable<T> where T : INSDSizable<T>
    {
        public int BinarySize { get; }
    }
}