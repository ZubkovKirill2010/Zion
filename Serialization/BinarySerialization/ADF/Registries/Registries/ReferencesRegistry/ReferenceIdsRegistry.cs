namespace Zion.Serialization.ADF
{
    public sealed class ReferenceIdsRegistry : IWritableRegistry
    {
        private static readonly Reference Null = new Reference(0, new(0, 0, -1));

        private readonly Dictionary<object, Reference> References;

        public int NewItemsCount { get; private set; }

        private uint LastId = 1u << 31;

        public ReferenceIdsRegistry()
        {
            References = new(ReferenceEqualityComparer.Instance);
        }


        public bool TryGetReference(object Value, out Reference Reference)
        {
            return References.TryGetValue(Value, out Reference);
        }
    }
}