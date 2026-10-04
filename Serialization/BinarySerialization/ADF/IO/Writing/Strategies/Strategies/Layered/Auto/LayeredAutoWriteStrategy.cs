namespace Zion.Serialization.ADF
{
    internal sealed class LayeredAutoWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly AutoWriter<T>[] Layers;

        public LayeredAutoWriteStrategy(List<ILayerWriteInfo> Layers)
        {
            this.Layers = CheckAndConvert(Layers);
        }


        public void Write(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            if (WriteStrategyHelper<T>.Setup(Context, Value, ref Target))
            {
                return;
            }
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i].Invoke(Context, Target, Value);
            }
        }


        private static AutoWriter<T>[] CheckAndConvert(List<ILayerWriteInfo> Layers)
        {
            var UsedNames = new HashSet<string>(Layers.Count * 6);
            var Result = new AutoWriter<T>[Layers.Count];

            for (int i = 0; i < Layers.Count; i++)
            {
                var Info = (AutoType<T>)Layers[i];
                var Fields = Info.Schema.Fields;

                for (int j = 0; j < Fields.Length; j++)
                {
                    var Name = Fields[i].Name;
                    if (!UsedNames.Add(Name))
                    {
                        throw new ADFRepeatedNameException(Name);
                    }
                }

                Result[i] = Info.Writer;
            }

            return Result;
        }
    }
}