namespace Zion
{
    public static class BinaryObject
    {
        public static T Load<T>(string Path) where T : IBinaryObject<T>
        {
            return IBinaryObject<T>.Load(Path);
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