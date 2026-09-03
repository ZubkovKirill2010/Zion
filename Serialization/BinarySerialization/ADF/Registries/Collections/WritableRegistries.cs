using System.Collections;

namespace Zion.Serialization.ADF
{
    public sealed class WritableRegistries : IEnumerable<WritableRegistryInfo>
    {
        private readonly Dictionary<string, WritableRegistryInfo> Registries;

        public readonly TypeAssociation     TypeAssociation;
        public readonly ReferenceIdsRegistry References;
        public readonly FormatIdRegistry     FormatRegistry;
        public readonly StringIdRegistry     StringRegistry;
        public readonly DataRegistry         DataRegistry;

        private ushort LastRegistryId = 32;


        public WritableRegistries()
        {
            TypeAssociation = new();
            References     = new();
            FormatRegistry = new();
            StringRegistry = new();
            DataRegistry   = new(StringRegistry);
            Registries     = new()
            {
                { "StringRegistry", new(1, StringRegistry) },
                { "FormatRegistry", new(2, FormatRegistry) },
                { "DataRegistry"  , new(3, DataRegistry  ) },
                { "References"    , new(4, References    ) }
            };
        }


        public bool TryGetRegistry<T>(string Name, out T Registry) where T : IWritableRegistry
        {
            if (Registries.TryGetValue(Name, out var WritableRegistry)
                && WritableRegistry is T Target)
            {
                Registry = Target;
            }
            Registry = default!;
            return false;
        }

        public void Add(string Name, IWritableRegistry Registry)
        {
            Registries.Add
            (
                Name.NotNull(),
                new(LastRegistryId++, Registry)
            );
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<WritableRegistryInfo> GetEnumerator()
        {
            return Registries.Values.GetEnumerator();
        }
    }
}