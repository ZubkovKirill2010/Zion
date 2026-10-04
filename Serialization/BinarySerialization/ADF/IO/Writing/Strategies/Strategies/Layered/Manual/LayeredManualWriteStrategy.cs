namespace Zion.Serialization.ADF
{
    internal sealed class LayeredManualWriteStrategy<T> : IWriteStrategy<T>
    {
        public LayeredManualWriteStrategy()
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