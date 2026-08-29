namespace Zion.Serialization.ADF
{
    public sealed class ADFWriter : BaseADFWriter
    {
        public readonly Stream BaseStream;

        
        public ADFWriter(Stream Stream) : base(new Arena<byte>(2048))
        {
            if (!Stream.NotNull().CanWrite)
            {
                throw new InvalidOperationException("Stream can not write");
            }
            BaseStream = Stream;
        }


        public void Flush()
        {

        }
    }
}