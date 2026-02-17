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


        public static void Write<T>(this BinaryWriter Writer, IEnumerable<T> Values) where T : IBinaryObject<T>
        {
            Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        }

        public static void Write(this BinaryWriter Writer, IEnumerable<object> Values)
        {
            Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        }

        public static void Write(this BinaryWriter Writer, IEnumerable Values)
        {
            Write(Writer, Values.Cast<object>(), (Writer, Item) => Writer.Write(Item));
        }


        public static void Write<T>(this BinaryWriter Writer, IEnumerable<T> Values, Action<BinaryWriter, T> Write)
        {
            Stream Stream = Writer.BaseStream;

            long Start = Stream.Position;
            int Count = 0;

            Writer.Seek(4, SeekOrigin.Current);

            foreach (T Item in Values)
            {
                Write(Writer, Item);
                Count++;
            }

            long End = Stream.Position;

            Stream.Position = Start;
            Writer.Write(Count);
            Stream.Position = End;
        }

        public static void Write<T, I>(this BinaryWriter Writer, IBinaryGeneric<T, I> Object, Action<BinaryWriter, I> Write) where T : IBinaryGeneric<T, I>
        {
            Object.Write(Writer, Write);
        }


        public static void Write<T>(this BinaryWriter Writer, IBinaryObject<T> Object) where T : IBinaryObject<T>
        {
            Object.Write(Writer);
        }

        public static void Write(this BinaryWriter Writer, object Object)
        {
            Type Type = Object.GetType();

            if (SerializationMap.TryGetValue(Type, out Action<BinaryWriter, object>? Write))
            {
                Write(Writer, Object);
            }

            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(IBinaryObject<>))
            {
                dynamic Item = Object;
                Item.Write(Writer);
            }

            throw new ArgumentException($"Instructions for writing an object of type '{Type}' does not exist");
        }


        public static void Write(this BinaryWriter Writer, IEnumerable<string> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<bool> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<byte> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<char> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<int> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<float> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<double> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<decimal> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<long> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<sbyte> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<ushort> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<uint> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
        public static void Write(this BinaryWriter Writer, IEnumerable<ulong> Values)
            => Write(Writer, Values, (Writer, Item) => Writer.Write(Item));
    }
}