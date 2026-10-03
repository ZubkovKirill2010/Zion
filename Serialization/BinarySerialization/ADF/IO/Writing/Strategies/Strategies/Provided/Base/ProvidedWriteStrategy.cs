namespace Zion.Serialization.ADF
{
    internal abstract class ProvidedWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly uint FormatId;
        private bool HasFormat;
        private DataFormat Format;


        public ProvidedWriteStrategy(uint FormatId)
        {
            this.FormatId = FormatId;
        }


        public static ProvidedWriteStrategy<T> GetStrategy(uint FormatId)
        {
            return typeof(T).IsValueType
                ? new SerializableStructWriteStrategy<T>(FormatId)
                : new SerializableClassWriteStrategy<T>(FormatId);
        }

        public static ProvidedWriteStrategy<T> GetStrategy(uint FormatId, IADFSerializer<T> Serializer)
        {
            return typeof(T).IsValueType
                ? new StructSerializerWriteStrategy<T>(FormatId, Serializer)
                : new ClassSerializerWriteStrategy<T>(FormatId, Serializer);
        }


        protected abstract StreamGroup GetGroupForData(ADFWritingContext Context, StreamGroup BaseGroup);

        protected abstract void Write(ADFWritingContext Context, StreamGroup Base, ADFObjectWriter Writer, T Value);

        protected virtual void OnWrited(ADFWritingContext Context, uint FormatId, T Value) { }

        protected virtual bool TryWriteReference(ADFWritingContext Context, ArenaStream BaseStream, T Value) => false;


        public void Write(ADFWritingContext Context, StreamGroup Group, T Value)
        {
            if (TryWriteReference(Context, Group.BaseStream, Value))
            {
                return;
            }

            var Target = GetGroupForData(Context, Group);

            if (HasFormat)
            {
                using var Writer = new ADFCheckingObjectWriter(Context, Target, Format);
                Write(Context, Group, Writer, Value);
                OnWrited(Context, FormatId, Value);
            }
            else
            {
                using var Writer = new ADFRecordObjectWriter(Context, Target, typeof(T));
                Write(Context, Group, Writer, Value);

                HasFormat = true;
                Format = Context.Registries.FormatRegistry.Clarify(FormatId, Writer.GetParameters());

                OnWrited(Context, FormatId, Value);
            }
        }
    }
}