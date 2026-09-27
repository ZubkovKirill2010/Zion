namespace Zion.Serialization.ADF
{
    internal sealed class StructSerializerWriteStrategy<T> : StructWriteStrategy<T>
    {
        private readonly IADFSerializer<T> Serializer;


        public StructSerializerWriteStrategy(ADFWritingContext Context, uint FormatId, IADFSerializer<T> Serializer)
            : base(Context, FormatId)
        {
            this.Serializer = Serializer.NotNull();
        }


        protected override void Write(StreamGroup Base, ADFObjectWriter Writer, T Value)
        {
            Serializer.Write(Writer, Value);
        }
    }
}