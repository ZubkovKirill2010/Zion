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
        public void Write(BinaryWriter Writer, Action<I> Write);

        /// <summary>
        /// Deserializes the object using custom reader for inner type.
        /// </summary>
        public static abstract T Read(BinaryReader Reader, Func<I> Read);
    }
}