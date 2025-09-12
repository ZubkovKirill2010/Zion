namespace Zion
{
    public static class BinaryObject
    {
        public static T Load<T>(string Path) where T : IBinaryObject<T>
        {
            using (FileStream Stream = new FileStream(Path, FileMode.Open))
            using (BinaryReader Reader = new BinaryReader(Stream))
            {
                return Reader.Read<T>();
            }
        }

        public static void Save<T>(this T Object, string Path) where T : IBinaryObject<T>
        {
            using (FileStream Stream = new FileStream(Path, FileMode.Create))
            using (BinaryWriter Writer = new BinaryWriter(Stream))
            {
                Writer.Write(Object);
            }
        }
    }

    public static class BinaryGeneric
    {
        public static T Load<T, I>(string Path, Func<BinaryReader, I> ReadObject) where T : IBinaryGeneric<T, I>
        {
            return IBinaryGeneric<T, I>.Load(Path, ReadObject);
        }
    }
}