namespace Zion.Serialization
{
    public interface IBinaryReader<T>
    {
        public T Read(BinaryReader Reader);
    }
}