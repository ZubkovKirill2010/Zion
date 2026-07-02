using System.Collections.Concurrent;

namespace Zion.Serialization
{
    public static class BinarySerializer
    {
        private static readonly ConcurrentDictionary<Type, object> Writers = new();
        private static readonly ConcurrentDictionary<Type, object> Readers = new();


        static BinarySerializer()
        {
            AddSerializer(new BinarySerializer<bool>    (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadBoolean() ));
            AddSerializer(new BinarySerializer<byte>    (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadByte()    ));
            AddSerializer(new BinarySerializer<sbyte>   (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadSByte()   ));
            AddSerializer(new BinarySerializer<char>    (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadChar()    ));
            AddSerializer(new BinarySerializer<decimal> (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadDecimal() ));
            AddSerializer(new BinarySerializer<double>  (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadDouble()  ));
            AddSerializer(new BinarySerializer<float>   (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadSingle()  ));
            AddSerializer(new BinarySerializer<int>     (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadInt32()   ));
            AddSerializer(new BinarySerializer<uint>    (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadUInt32()  ));
            AddSerializer(new BinarySerializer<long>    (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadInt64()   ));
            AddSerializer(new BinarySerializer<ulong>   (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadUInt64()  ));
            AddSerializer(new BinarySerializer<short>   (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadInt16()   ));
            AddSerializer(new BinarySerializer<ushort>  (static (Writer, Value) => Writer.Write(Value), static Reader => Reader.ReadUInt16()  ));
        }


        public static void AddSerializer<T>(IBinarySerializer<T> Serializer)
        {
            AddWriter(Serializer);
            AddReader(Serializer);
        }

        public static void AddWriter<T>(IBinaryWriter<T> ObjectWriter)
        {
            Writers[typeof(T)] = ObjectWriter;
        }

        public static void AddReader<T>(IBinaryReader<T> ObjectReader)
        {
            Readers[typeof(T)] = ObjectReader;
        }


        public static bool ContainsWriter<T>()
        {
            return Writers.ContainsKey(typeof(T));
        }

        public static bool ContainsReader<T>()
        {
            return Readers.ContainsKey(typeof(T));
        }


        public static bool TryGetWriter<T>(out IBinaryWriter<T> Writer)
        {
            if (Writers.TryGetValue(typeof(T), out object? Object))
            {
                Writer = (IBinaryWriter<T>)Object;
                return true;
            }
            Writer = default!;
            return false;
        }

        public static bool TryGetReader<T>(out IBinaryReader<T> Reader)
        {
            if (Readers.TryGetValue(typeof(T), out object? ObjectReader))
            {
                Reader = (IBinaryReader<T>)ObjectReader;
                return true;
            }
            Reader = default!;
            return false;
        }


        public static IBinaryWriter<T>? GetWriter<T>()
        {
            return TryGetWriter(out IBinaryWriter<T> Writer) ? Writer : null;
        }

        public static IBinaryReader<T>? GetReader<T>()
        {
            return TryGetReader(out IBinaryReader<T> Reader) ? Reader : null;
        }
    }
}