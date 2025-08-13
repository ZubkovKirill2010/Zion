namespace Zion
{
    public interface IBinaryObject<T> where T : IBinaryObject<T>
    {
        void Write(BinaryWriter Writer);
        static abstract T Read(BinaryReader Reader);
    }

    public interface IBinaryGeneric<T, I> where T : IBinaryGeneric<T, I>
    {
        void Write(BinaryWriter Writer, Action<BinaryWriter, I> WriteObject);
        static abstract T Read(BinaryReader Reader, Func<BinaryReader, I> ReadObject);
    }
}