namespace Zion
{
    public static class IBinaryObjectExtensions
    {
        public static void Save<T>(this IBinaryObject<T> Object, string FilePath) where T : IBinaryObject<T>
        {
            using (FileStream Stream = new FileStream(FilePath, FileMode.Create))
            using (BinaryWriter Writer = new BinaryWriter(Stream))
            {
                Writer.Write(Object);
            }
        }

        public static void Save<T, G>(this IBinaryGeneric<T, G> Generic, Action<BinaryWriter, G> Write, string FilePath) where T : IBinaryGeneric<T, G>
        {
            using (FileStream Stream = new FileStream(FilePath, FileMode.Create))
            using (BinaryWriter Writer = new BinaryWriter(Stream))
            {
                Writer.Write(Generic, Write);
            }
        }
    }
}