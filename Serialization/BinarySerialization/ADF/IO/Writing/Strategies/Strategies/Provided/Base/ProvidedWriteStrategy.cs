namespace Zion.Serialization.ADF
{
    public abstract class ProvidedWriteStrategy<T> : IWriteStrategy<T>
    {
        protected readonly ADFWritingContext Context;

        private bool HasFormat;
        private DataFormat Format;


        public ProvidedWriteStrategy(ADFWritingContext Context)
        {
            this.Context = Context.NotNull();
        }


        public static ProvidedWriteStrategy<T> GetStrategy(ADFWritingContext Context)
        {
            return typeof(T).IsValueType
                ? new SerializableStructWriteStrategy<T>(Context)
                : new SerializableClassWriteStrategy<T>(Context);
        }

        public static ProvidedWriteStrategy<T> GetStrategy(ADFWritingContext Context, IADFSerializer<T> Serializer)
        {
            return typeof(T).IsValueType
                ? new StructSerializerWriteStrategy<T>(Context, Serializer)
                : new ClassSerializerWriteStrategy<T>(Context, Serializer);
        }


        protected abstract StreamGroup GetGroupForData(StreamGroup BaseGroup);

        protected abstract void Write(ADFObjectWriter Writer, T Value);

        protected virtual bool TryWriteReference(ArenaStream BaseStream, T Value) => false;


        public void Write(StreamGroup Group, T Value)
        {
            if (TryWriteReference(Group.BaseStream, Value))
            {
                return;
            }

            var Target = GetGroupForData(Group);

            if (HasFormat)
            {
                using var Writer = new ADFCheckingObjectWriter(Context, Target, Format);
                Write(Writer, Value);
            }
            else
            {
                using var Writer = new ADFRecordObjectWriter(Context, Target, typeof(T));
                Write(Writer, Value);

                Format = Writer.BuildFormat();
                Context.Registries.FormatRegistry.Add(Format);

                HasFormat = true;
            }
        }
    }
}