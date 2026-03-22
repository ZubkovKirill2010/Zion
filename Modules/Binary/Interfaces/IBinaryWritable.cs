namespace Zion
{
    public interface IBinaryWritable
    {
        /// <summary>
        /// Serializes the object to binary stream.
        /// </summary>
        void Write(BinaryWriter Writer);
    }
}