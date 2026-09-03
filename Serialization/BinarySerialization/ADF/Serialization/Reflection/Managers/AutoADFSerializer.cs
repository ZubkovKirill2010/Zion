namespace Zion.Serialization.ADF
{
    public static class AutoADFSerializer
    {
        private static readonly Dictionary<Type, TypeSchema> Cache = new();


        public static AutoWriter<T> GetWriter<T>()
        {
            return GetWriter<T>(typeof(T));
        }

        public static AutoWriter<T> GetWriter<T>(Type Type)
        {
            if (!Cache.TryGetValue(Type, out var Base))
            {
                Base = TypeSchema.Create(Type);
            }

            return new AutoWriter<T>(Base);
        }
    }
}