using System.Collections;

namespace Zion.Serialization.ADF
{
    public sealed class WritableRegistries : IEnumerable<WritableRegistryInfo>
    {
        private readonly Dictionary<string, WritableRegistryInfo> Registries;

        public readonly StringIdRegistry StringRegistry;
        public readonly FormatIdRegistry FormatRegistry;

        private ushort LastRegistryId = 32;


        public WritableRegistries()
        {
            StringRegistry = new();
            FormatRegistry = new();
            Registries     = new()
            {
                { "StringRegistry", new(1, StringRegistry) },
                { "FormatRegistry", new(2, FormatRegistry) }
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