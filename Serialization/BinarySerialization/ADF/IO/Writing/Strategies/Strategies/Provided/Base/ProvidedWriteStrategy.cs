namespace Zion.Serialization.ADF
{
    public abstract class ProvidedWriteStrategy<T> : IWriteStrategy<T>
    {
        protected readonly ADFWritingContext Context;

        private bool HasFormat;
        private uint FormatId;
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

        protected abstract void Write(StreamGroup Base, ADFObjectWriter Writer, T Value);

        protected virtual void OnWrited(uint FormatId, T Value) { }

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
                Write(Group, Writer, Value);
                OnWrited(FormatId, Value);
            }
            else
            {
                using var Writer = new ADFRecordObjectWriter(Context, Target, typeof(T));
                Write(Group, Writer, Value);

                HasFormat = true;
                Format = Writer.BuildFormat();
                FormatId = Context.Registries.FormatRegistry.Add(Format);

                OnWrited(FormatId, Value);
            }
        }
    }
}