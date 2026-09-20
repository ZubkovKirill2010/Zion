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


        protected abstract ArenaStream GetStreamForData(StreamGroup BaseGroup);

        protected abstract void Write(ArenaStream BaseStream, ADFObjectWriter Writer, T Value);


        public virtual void Write(StreamGroup Group, T Value)
        {
            var Stream = GetStreamForData(Group);
            
            if (HasFormat)
            {
                using var Writer = new ADFCheckingObjectWriter(Context, Stream, Format);
                Write(Group.BaseStream, Writer, Value);
            }
            else
            {
                using var Writer = new ADFRecordObjectWriter(Context, Stream, typeof(T));

                Write(Group.BaseStream, Writer, Value);
                Format = Writer.BuildFormat();
                Context.Registries.FormatRegistry.Add(Format);

                HasFormat = true;
            }
        }
    }
}