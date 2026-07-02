namespace Zion.Serialization
{
    public interface IBinaryWriter<T>
    {
        public void Write(BinaryWriter Writer, T Value);
    }
}