namespace Zion.Serialization.ADF
{
    internal sealed class LayeredMixedWriteStrategy<T> : IWriteStrategy<T>
    {
        public LayeredMixedWriteStrategy(List<ILayerWriteInfo> Layers)
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