namespace Zion.Serialization.ADF
{
    internal sealed class ClassSerializerWriteStrategy<T> : ClassWriteStrategy<T>
    {
        private readonly IADFSerializer<T> Serializer;


        public ClassSerializerWriteStrategy(ADFWritingContext Context, uint FormatId, IADFSerializer<T> Serializer)
            : base(Context, FormatId)
        {
            this.Serializer = Serializer;
        }


        protected override void WriteValue(ADFObjectWriter Writer, T Value)
        {
            Serializer.Write(Writer, Value);
        }
    }
}