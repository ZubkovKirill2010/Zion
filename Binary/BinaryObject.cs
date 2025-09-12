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


        public static async Task<T> LoadAsync<T>(string Path) where T : IBinaryObject<T>
        {
            return await Task.Run(() => Load<T>(Path));
        }

        public static async Task SaveAsync<T>(T Object, string Path) where T : IBinaryObject<T>
        {
            await Task.Run(() => Save(Object, Path));
        }
    }

    public static class BinaryGeneric
    {
        public static T Load<T, I>(string Path, Func<BinaryReader, I> ReadObject) where T : IBinaryGeneric<T, I>
        {
            using (FileStream Stream = new FileStream(Path, FileMode.Open))
            using (BinaryReader Reader = new BinaryReader(Stream))
            {
                return T.Read(Reader, ReadObject);
            }
        }

        public static void Save<T, I>(this IBinaryGeneric<T, I> Object, string Path, Action<BinaryWriter, I> WriteObject) where T : IBinaryGeneric<T, I>
        {
            using (FileStream Stream = new FileStream(Path, FileMode.Create))
            using (BinaryWriter Writer = new BinaryWriter(Stream))
            {
                Object.Write(Writer, WriteObject);
            }
        }


        public static async Task<T> LoadAsync<T, I>(string Path, Func<BinaryReader, I> ReadObject) where T : IBinaryGeneric<T, I>
        {
            return await Task.Run(() => Load<T, I>(Path, ReadObject));
        }

        public static async Task SaveAsync<T, I>(T Object, string Path, Action<BinaryWriter, I> WriteObject) where T : IBinaryGeneric<T, I>
        {
            await Task.Run(() => Save(Object, Path, WriteObject));
        }
    }
}