namespace Zion
{
    public static class BinaryGeneric
    {
        public static T Load<T, I>(string Path, Func<I> Read) where T : IBinaryGeneric<T, I>
        {
            using (FileStream Stream = new FileStream(Path, FileMode.Open))
            using (BinaryReader Reader = new BinaryReader(Stream))
            {
                return T.Read(Reader, Read);
            }
        }

        public static void Save<T, I>(this IBinaryGeneric<T, I> Object, string Path, Action<I> Write) where T : IBinaryGeneric<T, I>
        {
            using (FileStream Stream = new FileStream(Path, FileMode.Create))
            using (BinaryWriter Writer = new BinaryWriter(Stream))
            {
                Object.Write(Writer, Write);
            }
        }


        public static async Task<T> LoadAsync<T, I>(string Path, Func<I> Read) where T : IBinaryGeneric<T, I>
        {
            return await Task.Run(() => Load<T, I>(Path, Read));
        }

        public static async Task SaveAsync<T, I>(T Object, string Path, Action<I> Write) where T : IBinaryGeneric<T, I>
        {
            await Task.Run(() => Save(Object, Path, Write));
        }
    }
}