namespace Zion.Serialization.NSD
{
    public interface INSDSizable<T> : INSDSizable, IBinarySerializable<T> where T : INSDSizable<T>
    {

    }

    public interface INSDSizable
    {
        public int BinarySize { get; }
    }
}