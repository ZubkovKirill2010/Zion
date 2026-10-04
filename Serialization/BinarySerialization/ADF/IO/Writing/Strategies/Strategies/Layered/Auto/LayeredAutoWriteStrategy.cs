namespace Zion.Serialization.ADF
{
    internal sealed class LayeredAutoWriteStrategy<T> : IWriteStrategy<T>
    {
        public LayeredAutoWriteStrategy()
        {

        }


        public void Write(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            if (WriteStrategyHelper<T>.Setup(Context, Value, ref Target))
            {
                return;
            }
        }
    }
}