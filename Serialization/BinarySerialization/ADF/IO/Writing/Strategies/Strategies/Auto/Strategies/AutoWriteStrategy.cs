namespace Zion.Serialization.ADF
{
    internal sealed class AutoWriteStrategy<T> : IWriteStrategy<T>
    {
        public readonly DataFormat Format;


        public AutoWriteStrategy()
        {
            Format = CreateFormat();
        }


        public void Write(ADFWritingContext Context, StreamGroup Target, T Value)
        {

        }


        private DataFormat CreateFormat()
        {
            //TODO AutoWriteStrategy.CreateFormat()
            throw new NotImplementedException();
        }
    }
}