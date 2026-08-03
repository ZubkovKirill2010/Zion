namespace Zion.Serialization.NSD
{
    public abstract class NSDWriteContext : IDisposable
    {
        private readonly HashSet<string> UsingKeys = new();

        protected readonly Stream Stream;
        protected readonly BinaryWriter Writer;

        public IEnumerable<string> Keys => UsingKeys;


        public NSDWriteContext(Stream Stream)
        {
            ArgumentException.ThrowIf(!Stream.NotNull().CanWrite, "The stream does not support writine");
            this.Stream = Stream;
            this.Writer = new BinaryWriter(Stream);
        }


        public void WritePrimitive<T>(string Key, T Value) where T : IBinaryWritable
        {
            CheckKeyAndValue(Key, Value);
            Writer.Write(Key);

            if (Value is INSDSizable Sizable)
            {
                Writer.Write(Sizable.BinarySize);
                Writer.Write(Value);
            }
            else
            {
                WritePrimitiveSafe(Value);
            }
        }

        public void Write<T>(string Key, T Value) where T : INSDContainer<T>
        {
            CheckKeyAndValue(Key, Value);
            Writer.Write(Key);
            WriteSafe(Value);
        }

        public void Write<T>(string Key, T Value, IBinaryWriter<T>? ObjectWriter = null)
        {
            CheckKeyAndValue(Key, Value);
            ObjectWriter ??= BinarySerializer.GetWriter<T>();
            WriteSafe(Value, ObjectWriter);
        }


        private void Write<T>(string Key, T Value, int Size, IBinaryWriter<T> ObjectWriter)
        {
            CheckKeyAndValue(Key, Value);

            Writer.Write(Key);
            Writer.Write(Size);

            ObjectWriter.Write(Writer, Value);
        }

        private void Write<T>(string Key, ICollection<T> Collection, int Size, IBinaryWriter<T> ObjectWriter)
        {
            CheckKeyAndValue(Key, Collection);

            int Count = Collection.Count;

            Writer.Write(Key);
            Writer.Write(Count * Size + 4);

            Writer.WriteCollection(Collection, ObjectWriter);
        }


        public void Write(string Key, bool Value)
        {
            Write(Key, Value, sizeof(bool), bool.Serializer);
        }

        public void Write(string Key, byte Value)
        {
            Write(Key, Value, sizeof(byte), byte.Serializer);
        }

        public void Write(string Key, sbyte Value)
        {
            Write(Key, Value, sizeof(sbyte), sbyte.Serializer);
        }

        public void Write(string Key, char Value)
        {
            Write(Key, Value, sizeof(char), char.Serializer);
        }

        public void Write(string Key, decimal Value)
        {
            Write(Key, Value, sizeof(decimal), decimal.Serializer);
        }

        public void Write(string Key, double Value)
        {
            Write(Key, Value, sizeof(double), double.Serializer);
        }

        public void Write(string Key, float Value)
        {
            Write(Key, Value, sizeof(float), float.Serializer);
        }

        public void Write(string Key, int Value)
        {
            Write(Key, Value, sizeof(int), int.Serializer);
        }

        public void Write(string Key, uint Value)
        {
            Write(Key, Value, sizeof(uint), uint.Serializer);
        }

        public void Write(string Key, long Value)
        {
            Write(Key, Value, sizeof(long), long.Serializer);
        }

        public void Write(string Key, ulong Value)
        {
            Write(Key, Value, sizeof(ulong), ulong.Serializer);
        }

        public void Write(string Key, short Value)
        {
            Write(Key, Value, sizeof(short), short.Serializer);
        }

        public void Write(string Key, ushort Value)
        {
            Write(Key, Value, sizeof(ushort), ushort.Serializer);
        }

        public void Write(string Key, string Value)
        {
            Write(Key, Value, string.Serializer);
        }


        public void Write(string Key, ICollection<bool> Collection)
        {
            Write(Key, Collection, sizeof(bool), bool.Serializer);
        }

        public void Write(string Key, ICollection<byte> Collection)
        {
            Write(Key, Collection, sizeof(byte), byte.Serializer);
        }

        public void Write(string Key, ICollection<sbyte> Collection)
        {
            Write(Key, Collection, sizeof(sbyte), sbyte.Serializer);
        }

        public void Write(string Key, ICollection<char> Collection)
        {
            Write(Key, Collection, sizeof(char), char.Serializer);
        }

        public void Write(string Key, ICollection<decimal> Collection)
        {
            Write(Key, Collection, sizeof(decimal), decimal.Serializer);
        }

        public void Write(string Key, ICollection<double> Collection)
        {
            Write(Key, Collection, sizeof(double), double.Serializer);
        }

        public void Write(string Key, ICollection<float> Collection)
        {
            Write(Key, Collection, sizeof(float), float.Serializer);
        }

        public void Write(string Key, ICollection<int> Collection)
        {
            Write(Key, Collection, sizeof(int), int.Serializer);
        }

        public void Write(string Key, ICollection<uint> Collection)
        {
            Write(Key, Collection, sizeof(uint), uint.Serializer);
        }

        public void Write(string Key, ICollection<long> Collection)
        {
            Write(Key, Collection, sizeof(long), long.Serializer);
        }

        public void Write(string Key, ICollection<ulong> Collection)
        {
            Write(Key, Collection, sizeof(ulong), ulong.Serializer);
        }

        public void Write(string Key, ICollection<short> Collection)
        {
            Write(Key, Collection, sizeof(short), short.Serializer);
        }

        public void Write(string Key, ICollection<ushort> Collection)
        {
            Write(Key, Collection, sizeof(ushort), ushort.Serializer);
        }

        public void Write(string Key, ICollection<string> Collection)
        {
            Write
            (
                Key,
                Collection,
                new CollectionWriter<string>(static (Writer, Value) => Writer.Write(Value))
            );
        }


        //Only Size + Setters
        protected abstract void WritePrimitiveSafe<T>(T Value) where T : IBinaryWritable;

        protected abstract void WriteSafe<T>(T Value) where T : INSDContainer<T>;

        protected abstract void WriteSafe<T>(T Value, IBinaryWriter<T>? ObjectWriter = null);

        protected virtual void Dispose(bool Disposing) { }


        public void Dispose()
        {
            Dispose(true);
            Stream.Dispose();
        }


        private void CheckKeyAndValue<T>(string Key, T Value)
        {
            ArgumentNullException.ThrowIfNull(Value);
            if (!UsingKeys.Add(Key.NotNull()))
            {
                throw new ArgumentException($"Key '{Key}' already exists");
            }
        }
    }
}