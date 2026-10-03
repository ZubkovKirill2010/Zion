namespace Zion.Serialization.ADF
{
    internal sealed class LayeredWriteStrategy<T> : IWriteStrategy<T>
    {
        #region Data
        private readonly IWriteStrategy<T>[] Strategies;

        #endregion

        #region Constructors
        public LayeredWriteStrategy(IWriteStrategy<T>[] Strategies)
        {
            this.Strategies = Strategies.NotNull();
        }

        #endregion

        #region PublicMethods
        //public static bool TryCreate(T Value, out IWriteStrategy<T> Strategy)
        //{
            //var Type = Value!.GetType();

            //if (!DataFormat.HasBase(Value!.GetType()))
            //{
            //    Strategy = default!;
            //    return false;
            //}

            //var Layers = EnumerateHierarchy(Type).Reverse().ToList();
            //var Strategies = new List<IWriteStrategy<T>>(Layers.Count);
            //var AllAuto = true;

            //foreach (var Layer in Layers)
            //{
            //    IWriteStrategy<T>? LayerStrategy = null;

            //    if (ADFSerializers.TryGetSerializer<T>(Layer, out var serializer))
            //    {
            //        LayerStrategy = new SerializerLayerStrategy<T>(serializer);
            //        AllAuto = false;
            //    }
            //    else if (HasOwnInterface(Layer, typeof(IADFWritable)))
            //    {
            //        LayerStrategy = new SerializableLayerStrategy<T>(Layer);
            //        AllAuto = false;
            //    }
            //    else
            //    {
            //        LayerStrategy = new AutoWriteStrategy<T>();
            //    }

            //    Strategies.Add(LayerStrategy);

            //    if (LayerStrategy.Coverage == Coverage.AllBaseLayers)
            //    {
            //        break;
            //    }
            //}

            //if (AllAuto)
            //{
            //    Strategy = default!;
            //    return false;
            //}

            //Strategy = new LayeredWriteStrategy<T>(Strategies.ToReversedArray());
            //return true;
        //}

        #endregion

        #region IWriteStrategy
        public void Write(ADFWritingContext Context, StreamGroup Target, T Value)
        {

        }

        #endregion

        #region PrivateMethods
        private static IEnumerable<Type> EnumerateHierarchy(Type? Type)
        {
            while (DataFormat.HasBase(Type))
            {
                yield return Type;
                Type = Type.BaseType;
            }
        }

        private static bool HasOwnInterface(Type Layer, Type Interface)
        {
            if (!Interface.IsAssignableFrom(Layer))
            {
                return false;
            }

            var Map = Layer.GetInterfaceMap(Interface);

            foreach (var Method in Map.TargetMethods)
            {
                if (Method.DeclaringType == Layer)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}