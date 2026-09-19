namespace Zion.Serialization.ADF
{
    public sealed class TypeAssociation
    {
        private readonly Dictionary<Type, TypeInfo> Data;


        public TypeAssociation()
        {
            Data = new();
        }


        public TypeInfo this[Type Type] => Data[Type];


        public bool TryGetInfo(Type Type, out TypeInfo Info)
        {
            return Data.TryGetValue(Type, out Info);
        }

        public bool TryGetFormatId(Type Type, out uint FormatId)
        {
            if (Data.TryGetValue(Type, out var Info))
            {
                FormatId = Info.FormatId;
                return true;
            }
            FormatId = default!;
            return false;
        }


        public bool Add(Type Type, uint FormatId)
        {
            return Data.TryAdd(Type, FormatId);
        }

        public uint GetOrAdd(Type Type)
        {
            if (Data.TryGetValue(Type, out uint FormatId))
            {
                return FormatId;
            }
        }
    }
}