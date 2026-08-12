namespace Zion.Serialization.TDC
{
    public interface IFormat<T>
        : IFormat, IBinarySerializable<T> where T : IFormat<T> { }
}