namespace Zion
{
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
        abstract static T Read(BinaryReader Reader, Func<BinaryReader, I> ReadObject);
    }
}