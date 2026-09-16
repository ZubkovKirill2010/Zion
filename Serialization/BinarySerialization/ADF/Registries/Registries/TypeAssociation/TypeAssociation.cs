namespace Zion.Serialization.ADF
{
    public sealed class TypeAssociation
    {
        private readonly Dictionary<Type, uint> Formats;


        public TypeAssociation()
        {
            Formats = new();
        }


        public uint this[Type Type] => Formats[Type];


        public bool TryGetFormatId(Type Type, out uint FormatId)
        {
            return Formats.TryGetValue(Type, out FormatId);
        }

        public bool Add(Type Type, uint FormatId)
        {
            return Formats.TryAdd(Type, FormatId);
        }

        public uint GetOrAdd(Type Type)
        {
            if (Formats.TryGetValue(Type, out uint FormatId))
            {
                return FormatId;
            }
        }
    }
}