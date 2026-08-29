namespace Zion.Serialization.ADF
{
    public sealed class ReadableRegistries
    {
        public sealed class WritableRegistries
        {
            private readonly Dictionary<string, ReadableRegistryInfo> Registries;

            public readonly StringRegistry StringRegistry;
            public readonly FormatRegistry FormatRegistry;


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


            public bool TryGetRegistry<T>(string Name, out T Registry) where T : IReadableRegistry
            {
                if (Registries.TryGetValue(Name, out var WritableRegistry)
                    && WritableRegistry is T Target)
                {
                    Registry = Target;
                }
                Registry = default!;
                return false;
            }
        }
    }
}