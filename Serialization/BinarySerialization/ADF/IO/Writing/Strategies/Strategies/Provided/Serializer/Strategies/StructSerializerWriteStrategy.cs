namespace Zion.Serialization.ADF
{
    internal sealed class StructSerializerWriteStrategy<T> : StructWriteStrategy<T>
    {
        private readonly IADFSerializer<T> Serializer;


        public StructSerializerWriteStrategy(ADFWritingContext Context, IADFSerializer<T> Serializer)
            : base(Context)
        {
            this.Serializer = Serializer.NotNull();
        }


        protected override void Write(ArenaStream BaseStream, ADFObjectWriter Writer, T Value)
        {
            
        }
    }
}