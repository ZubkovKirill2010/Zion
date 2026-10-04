namespace Zion.Serialization.ADF
{
    internal sealed class WriteStrategies
    {
        #region Data
        private readonly Dictionary<Type, WriteEntry> Strategies = new();

        #endregion

        #region PublicMethods
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

        #endregion

        #region Creation
        private WriteEntry<T> Create<T>(ADFWritingContext Context, Type Type)
        {
            if (TryCreateLayered<T>(Context, Type, out var LayeredStrategy))
            {
                var LayeredFormatId = GetOrAddDeferred(Context, Type);
                return new
                (
                    LayeredFormatId,
                    LayeredStrategy
                );
            }

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

        private static bool TryCreateLayered<T>(ADFWritingContext Context, Type Type, out IWriteStrategy<T> Strategy)
        {
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} is not {typeof(T)}");
            }

            if (!DataFormat.HasBase(Type))
            {
                Strategy = default!;
                return false;
            }

            var Layers = new List<ILayerWriteInfo>(10);
            var Kind = WriteKind.None;

            foreach (var LayerType in EnumerateHierarchy(Type))
            {
                //TODO: Manual strategies
                if (ADFSerializers.TryGetSerializer<T>(Type, out var Serializer))
                {
                    Kind |= WriteKind.Manual;
                }
                else if (HasOwnInterface(LayerType, typeof(IADFWritable)))
                {
                    Kind |= WriteKind.Manual;
                }
                else
                {
                    Kind |= WriteKind.Auto;

                    var AutoType = AutoSerializationCache.GetOrAdd<T>(Context, Type);
                    Layers.Add(AutoType);
                }
            }

            Strategy = Kind switch
            {
                WriteKind.Auto   => new LayeredAutoWriteStrategy<T>(Layers),
                WriteKind.Manual => new LayeredManualWriteStrategy<T>(),
                WriteKind.Mixed  => new LayeredMixedWriteStrategy<T>(),
                _ => throw new Exception()
            };
            return true;
        }

        #endregion

        #region PrivateMethods
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
            throw new NotImplementedException(); //TODO: HasOwnInterface
        }


        private static void ThrowIfNotAssignable<T>(Type Type)
        {
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} is not assignable from {typeof(T)}");
            }
        }

        #endregion
    }
}