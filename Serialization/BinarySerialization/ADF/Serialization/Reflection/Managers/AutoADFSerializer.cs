namespace Zion.Serialization.ADF
{
    internal static class AutoADFSerializer
    {
        private static readonly Dictionary<Type, TypeSchema> Cache = new();

        public static TypeSchema GetSchema<T>(Type Type)
        {
            return Cache.TryGetValue(Type, out var Cached)
                ? Cached
                : Cache.AddAndReturn(Type, TypeSchema.Create(Type));
        }
    }
}