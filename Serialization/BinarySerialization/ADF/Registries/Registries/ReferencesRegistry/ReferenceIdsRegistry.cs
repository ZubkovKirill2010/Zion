namespace Zion.Serialization.ADF
{
    public sealed class ReferenceIdsRegistry : IWritableRegistry
    {
        private static readonly Reference Null = new Reference(0, new(0, 0, -1));

        private readonly Dictionary<object, Reference> References;

        private uint LastId = 1u << 31;

        public bool IsChanged { get; private set; }


        public ReferenceIdsRegistry()
        {
            References = new(ReferenceEqualityComparer.Instance);
        }


        public void Write(ADFObjectWriter Writer)
        {
            //TODO: IWritableRegistry.Write
        }


        public bool TryGetReference(object? Value, out Reference Reference)
        {
            if (Value is null)
            {
                Reference = Null;
                return true;
            }
            return References.TryGetValue(Value, out Reference);
        }
    }
}