namespace Zion.Serialization.NSD
{
    public abstract class NSDWriteContext
    {
        private readonly HashSet<string> UsingKeys = new();

        protected readonly Stream Stream;
        protected readonly BinaryWriter Writer;


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


        //Only Size + Data
        protected abstract void WritePrimitiveSafe<T>(T Value) where T : IBinaryWritable;

        protected abstract void WriteSafe<T>(T Value) where T : INSDContainer<T>;

        protected abstract void WriteSafe<T>(T Value, IBinaryWriter<T>? ObjectWriter = null);


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