namespace Zion.Serialization
{
    public interface IBinaryWritable
    {
        /// <summary>
        /// Serializes the object to binary stream.
        /// </summary>
        public void Write(BinaryWriter Writer);
    }

    public interface IBinaryWritable<T>
    {
        public void Write(BinaryWriter Writer, Action<T> Write);
    }
}