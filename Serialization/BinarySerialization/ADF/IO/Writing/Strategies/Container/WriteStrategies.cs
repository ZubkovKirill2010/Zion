namespace Zion.Serialization.ADF
{
    public sealed class WriteStrategies
    {
        private readonly Dictionary<Type, WriteEntry> Strategies = new();


        internal WriteEntry<T> GetEntry<T>(Type Type)
        {
            ThrowIfNotAssignable<T>(Type);

            if (Strategies.TryGetValue(Type, out var Generalized))
            {
                return (WriteEntry<T>)Generalized;
            }

            var Created = Create<T>(Type);
            Strategies.Add(Type, Created);
            return Created;
        }

        private WriteEntry<T> Create<T>(Type Type)
        {
            ////TODO: Layred

            //if (ADFSerializers.TryGetSerializer<T>(Type, out var Serializer))
            //{
            //    var SerializableFormatId = GetOrAddDeferred(Type);
            //    return new
            //    (
            //        SerializableFormatId,
            //        ProvidedWriteStrategy<T>.GetStrategy(SerializableFormatId, Serializer)
            //    );
            //}

            //if (Type.IsAssignableTo(typeof(IADFWritable)))
            //{
            //    var WritableFormatId = GetOrAddDeferred(Type);
            //    return new
            //    (
            //        WritableFormatId,
            //        ProvidedWriteStrategy<T>.GetStrategy(WritableFormatId)
            //    );
            //}

            //var Strategy = new AutoWriteStrategy<T>();
            //var FormatId = FormatRegistry.Add(Strategy.Format);

            //return new(FormatId, Strategy);
            throw new NotImplementedException();
        }

        private static void ThrowIfNotAssignable<T>(Type Type)
        {
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} is not assignable from {typeof(T)}");
            }
        }
    }
}