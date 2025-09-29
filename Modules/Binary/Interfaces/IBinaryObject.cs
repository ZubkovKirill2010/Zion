namespace Zion
{
    /// <summary>
    /// Interface for objects that can be serialized to binary format.
    /// </summary>
    /// <typeparam name="T">Implementing type</typeparam>
    public interface IBinaryObject<T> where T : IBinaryObject<T>
    {
        /// <summary>
        /// Serializes the object to binary stream.
        /// </summary>
        void Write(BinaryWriter Writer);

        /// <summary>
        /// Deserializes the object from binary stream.
        /// </summary>
        abstract static T Read(BinaryReader Reader);
    }
}