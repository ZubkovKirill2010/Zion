namespace Zion.Serialization.ADF
{
    internal sealed class ClassSerializerWriteStrategy<T> : ClassWriteStrategy<T>
    {
        private readonly IADFSerializer<T> Serializer;


        public ClassSerializerWriteStrategy(ADFWritingContext Context, IADFSerializer<T> Serializer)
            : base(Context)
        {
            this.Serializer = Serializer;
        }


        protected override void Write(ADFObjectWriter Writer, T Value)
        {
            Serializer.Write(Writer, Value);
        }
    }
}