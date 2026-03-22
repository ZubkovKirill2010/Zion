using System.Collections;
using System.Collections.Concurrent;

namespace Zion
{
    public static class BinaryWriterExtensions
    {
        private static readonly ConcurrentDictionary<Type, Action<BinaryWriter, object>> SerializationMap = new();

        extension(BinaryWriter)
        {
            public static void AddSample<T>(Action<BinaryWriter, T> Write)
            {
                SerializationMap.TryAdd(typeof(T), (Writer, Item) => Write(Writer, (T)Item));
            }
        }

        static BinaryWriterExtensions()
        {
            BinaryWriter.AddSample<string>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<bool>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<byte>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<char>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<int>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<float>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<double>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<decimal>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<long>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<sbyte>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<ushort>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<uint>((Writer, Item) => Writer.Write(Item));
            BinaryWriter.AddSample<ulong>((Writer, Item) => Writer.Write(Item));
        }


        public static void Write<T>(this BinaryWriter Writer, ICollection<T> Values) where T : IBinaryWritable
        {
            Write(Writer, Values, Item => Writer.Write(Item));
        }

        public static void Write(this BinaryWriter Writer, ICollection<object> Values)
        {
            Write(Writer, Values, Writer.Write);
        }

        public static void Write(this BinaryWriter Writer, ICollection Values)
        {
            Writer.Write(Values.Count);
            foreach (object Item in Values)
            {
                Writer.Write(Item);
            }
        }


        public static void Write<T>(this BinaryWriter Writer, ICollection<T> Values, Action<T> Write)
        {
            Writer.Write(Values.Count);
            foreach (T Item in Values)
            {
                Write(Item);
            }
        }

        public static void Write<T, I>(this BinaryWriter Writer, IBinaryGeneric<T, I> Object, Action<I> Write) where T : IBinaryGeneric<T, I>
        {
            Object.Write(Writer, Write);
        }


        public static void Write<T>(this BinaryWriter Writer, IBinarySerializable<T> Object) where T : IBinarySerializable<T>
        {
            Object.Write(Writer);
        }

        public static void Write(this BinaryWriter Writer, object Object)
        {
            if (Object is IBinaryWritable Writable)
            {
                Writable.Write(Writer);
                return;
            }

            Type Type = Object.GetType();

            if (SerializationMap.TryGetValue(Type, out Action<BinaryWriter, object>? Write))
            {
                Write(Writer, Object);
                return;
            }

            throw new ArgumentException($"Instructions for writing an object of type '{Type}' does not exist");
        }


        public static void Write(this BinaryWriter Writer, ICollection<string> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<bool> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<byte> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<char> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<int> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<float> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<double> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<decimal> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<long> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<sbyte> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<ushort> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<uint> Values)
            => Write(Writer, Values, Writer.Write);
        public static void Write(this BinaryWriter Writer, ICollection<ulong> Values)
            => Write(Writer, Values, Writer.Write);
    }
}