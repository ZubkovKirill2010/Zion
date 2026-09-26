namespace Zion.Serialization.ADF
{
    public sealed class ReferenceIdsRegistry : IWritableRegistry
    {
        private static readonly Reference Null = new Reference(0, new(ADFPrimitives.Object, 0u, -1u));

        private readonly Dictionary<object, Reference> References; //Optimize: Заменить на слабую ссылку

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


        public void Add(object Value, DataDefinition Definition)
        {
            References.Add(Value, new(LastId++, Definition));
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