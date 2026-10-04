namespace Zion.Serialization.ADF
{
    internal sealed class LayeredManualWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly IADFSerializer<T>[] Serializers;

        public LayeredManualWriteStrategy(List<ILayerWriteInfo> Layers)
        {
            Serializers = Cast(Layers);
        }


        public void Write(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            if (WriteStrategyHelper<T>.Setup(Context, Value, ref Target))
            {
                return;
            }
            using (var Writer = new ADFLayeredObjectWriter<T>(Context, Target, Serializers))
            {
                Writer.Serialize(Value);
            }
        }


        private static IADFSerializer<T>[] Cast(List<ILayerWriteInfo> Layers)
        {
            var Result = new IADFSerializer<T>[Layers.Count];
            for (int i = 0; i < Layers.Count; i++)
            {
                Result[i] = (IADFSerializer<T>)Layers[i];
            }
            return Result;
        }
    }
}