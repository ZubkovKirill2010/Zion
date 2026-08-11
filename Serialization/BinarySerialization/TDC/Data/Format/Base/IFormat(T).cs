namespace Zion.Serialization.TDC
{
    public interface IFormat<T>
        : IBinarySerializable<T> where T : IFormat<T> { }
}