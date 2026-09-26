namespace Zion.Serialization.ADF
{
    public sealed class TypeAssociation
    {
        private readonly Dictionary<Type, uint> Data;

        private uint NextId;


        public TypeAssociation()
        {
            Data = new();
        }


        public uint this[Type Type] => Data[Type];


        public bool TryGetFormatId(Type Type, out uint FormatId)
        {
            return Data.TryGetValue(Type, out FormatId);
        }

        public bool Add(Type Type, uint FormatId)
        {
            return Data.TryAdd(Type, FormatId);
        }

        public uint GetOrAddDeferred(Type Type, FormatRegistry FormatRegistry)
        {
            if (Data.TryGetValue(Type, out var Existing))
            {
                return Existing;
            }

            var Base = DataFormat.HasBase(Type) 
                ? GetOrAddDeferred(Type, FormatRegistry)
                : ADFPrimitives.Object;

            return Data.AddAndReturn(Type, FormatRegistry.AddDeferred(Base));
        }
    }
}