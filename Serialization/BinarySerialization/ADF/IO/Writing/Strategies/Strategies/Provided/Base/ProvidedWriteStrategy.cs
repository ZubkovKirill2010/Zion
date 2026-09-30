namespace Zion.Serialization.ADF
{
    internal abstract class ProvidedWriteStrategy<T> : IWriteStrategy<T>
    {
        protected readonly ADFWritingContext Context;

        private readonly uint FormatId;
        private bool HasFormat;
        private DataFormat Format;


        public ProvidedWriteStrategy(ADFWritingContext Context, uint FormatId)
        {
            this.Context = Context.NotNull();
            this.FormatId = FormatId;
        }


        public static ProvidedWriteStrategy<T> GetStrategy(ADFWritingContext Context, uint FormatId)
        {
            return typeof(T).IsValueType
                ? new SerializableStructWriteStrategy<T>(Context, FormatId)
                : new SerializableClassWriteStrategy<T>(Context, FormatId);
        }

        public static ProvidedWriteStrategy<T> GetStrategy(ADFWritingContext Context, uint FormatId, IADFSerializer<T> Serializer)
        {
            return typeof(T).IsValueType
                ? new StructSerializerWriteStrategy<T>(Context, FormatId, Serializer)
                : new ClassSerializerWriteStrategy<T>(Context, FormatId, Serializer);
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
                Format = Context.Registries.FormatRegistry.Clarify(FormatId, Writer.GetParameters());

                OnWrited(FormatId, Value);
            }
        }
    }
}