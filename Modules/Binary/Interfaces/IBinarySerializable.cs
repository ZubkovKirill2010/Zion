namespace Zion
{
    /// <summary>
    /// Interface for objects that can be serialized to binary format.
    /// </summary>
    /// <typeparam name="T">Implementing type</typeparam>
    public interface IBinarySerializable<T> : IBinaryWritable where T : IBinarySerializable<T>
    {
        /// <summary>
        /// Deserializes the object from binary stream.
        /// </summary>
        abstract static T Read(BinaryReader Reader);
    }
}