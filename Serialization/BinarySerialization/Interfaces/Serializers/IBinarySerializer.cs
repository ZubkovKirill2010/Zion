namespace Zion.Serialization
{
    public interface IBinarySerializer<T>
      : IBinaryWriter<T>,
        IBinaryReader<T>
    { }
}