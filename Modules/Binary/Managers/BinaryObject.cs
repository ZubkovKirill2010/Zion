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
}