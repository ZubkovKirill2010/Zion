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
        static abstract T Read(BinaryReader Reader);
    }

    /// <summary>
    /// Generic version of binary serialization interface.
    /// </summary>
    public interface IBinaryGeneric<T, I> where T : IBinaryGeneric<T, I>
    {
        /// <summary>
        /// Serializes the object using custom writer for inner type.
        /// </summary>
        void Write(BinaryWriter Writer, Action<BinaryWriter, I> WriteObject);

        /// <summary>
        /// Deserializes the object using custom reader for inner type.
        /// </summary>
        static abstract T Read(BinaryReader Reader, Func<BinaryReader, I> ReadObject);
    }
}