namespace Zion.Serialization.ADF
{
    internal sealed class WriteStrategies
    {
        private readonly Dictionary<Type, WriteEntry> Strategies = new();


        public WriteEntry<T> GetEntry<T>(ADFWritingContext Context, Type Type)
        {
            ThrowIfNotAssignable<T>(Type);

            if (Strategies.TryGetValue(Type, out var Generalized))
            {
                return (WriteEntry<T>)Generalized;
            }

            var Created = Create<T>(Context, Type);
            Strategies.Add(Type, Created);
            return Created;
        }

        private WriteEntry<T> Create<T>(ADFWritingContext Context, Type Type)
        {
            //TODO: Layred

            if (ADFSerializers.TryGetSerializer<T>(Type, out var Serializer))
            {
                var SerializableFormatId = GetOrAddDeferred(Context, Type);
                return new
                (
                    SerializableFormatId,
                    ProvidedWriteStrategy<T>.GetStrategy(SerializableFormatId, Serializer)
                );
            }

            if (Type.IsAssignableTo(typeof(IADFWritable)))
            {
                var WritableFormatId = GetOrAddDeferred(Context, Type);
                return new
                (
                    WritableFormatId,
                    ProvidedWriteStrategy<T>.GetStrategy(WritableFormatId)
                );
            }

            var AutoType = AutoSerializationCache.GetOrAdd<T>(Context, Type);
            var FormatId = GetFormatId(Context, AutoType.Schema, Type);
            var Strategy = new AutoWriteStrategy<T>(AutoType.Writer);

            return new(FormatId, Strategy);
        }


        private static uint GetOrAddDeferred(ADFWritingContext Context, Type Type)
        {
            return Context.TypeAssociation.GetOrAddDeferred(Type, Context.Registries.FormatRegistry);
        }

        private static uint GetFormatId(ADFWritingContext Context, TypeSchema Schema, Type Type)
        {
            var TypeAssociation = Context.TypeAssociation;

            uint Create()
            {
                var Format = DataFormatBuilder.Build(Schema, Context);
                return Context.Registries.FormatRegistry.Add(Format);
            }

            return TypeAssociation.GetOrAdd(Type, Create);
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