namespace Zion
{
    public static class BinaryWriterExtensions
    {
        public static void Write<T>(this BinaryWriter Writer, ICollection<T> Collection) where T : IBinaryObject<T>
        {
            Writer.Write(Collection.Count);
            foreach (T Item in Collection)
            {
                Item.Write(Writer);
            }
        }

        public static void Write<T>(this BinaryWriter Writer, ICollection<T> Collection, Action<BinaryWriter, T> Write)
        {
            Writer.Write(Collection.Count);
            foreach (T Item in Collection)
            {
                Write(Writer, Item);
            }
        }


        public static void Write<T>(this BinaryWriter Writer, IBinaryObject<T> Object) where T : IBinaryObject<T>
        {
            Object.Write(Writer);
        }

        public static void Write<T, I>(this BinaryWriter Writer, IBinaryGeneric<T, I> Object, Action<BinaryWriter, I> Write) where T : IBinaryGeneric<T, I>
        {
            Object.Write(Writer, Write);
        }


        public static void Write(this BinaryWriter Writer, ICollection<string> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<bool> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<byte> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<char> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<int> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<float> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<double> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<decimal> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<long> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<sbyte> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<ushort> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<uint> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, ICollection<ulong> Collection)
            => Write(Writer, Collection, (Writer, Item) => Writer.Write(Item));
    }
}