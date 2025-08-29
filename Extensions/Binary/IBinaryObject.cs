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


        void Save(string FilePath)
        {
            using FileStream Stream = new FileStream(FilePath, FileMode.Create);
            using BinaryWriter Writer = new BinaryWriter(Stream);
            Write(Writer);
        }

        static T Load(string FilePath)
        {
            using FileStream Stream = new FileStream(FilePath, FileMode.Open);
            using BinaryReader Reader = new BinaryReader(Stream);
            return T.Read(Reader);
        }
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
        abstract static T Read(BinaryReader Reader, Func<BinaryReader, I> ReadObject);


        void Save(string FilePath, Action<BinaryWriter, I> WriteObject)
        {
            using FileStream Stream = new FileStream(FilePath, FileMode.Create);
            using BinaryWriter Writer = new BinaryWriter(Stream);
            Write(Writer, WriteObject);
        }

        static T Load(string FilePath, Func<BinaryReader, I> ReadObject)
        {
            using FileStream Stream = new FileStream(FilePath, FileMode.Open);
            using BinaryReader Reader = new BinaryReader(Stream);
            return T.Read(Reader, ReadObject);
        }
    }
}