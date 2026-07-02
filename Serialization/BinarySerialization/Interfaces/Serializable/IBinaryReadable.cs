namespace Zion.Serialization
{
    public interface IBinaryReadable<T> where T : IBinaryReadable<T>
    {
        /// <summary>
        /// Deserializes the object from binary stream.
        /// </summary>
        public static abstract T Read(BinaryReader Reader);
    }

    public interface IBinaryReadable<T, I> where T : IBinaryReadable<T, I>
    {
        /// <summary>
        /// Deserializes the object from binary stream.
        /// </summary>
        public static abstract T Read(BinaryReader Reader, Func<I> Read);
    }
}