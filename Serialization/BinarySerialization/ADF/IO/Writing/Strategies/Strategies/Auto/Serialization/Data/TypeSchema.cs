namespace Zion.Serialization.ADF
{
    public readonly struct TypeSchema
    {
        #region Cache
        private static readonly Dictionary<Type, TypeSchema> Cache = new(64);

        #endregion

        
        #region PublicMethods
        public static TypeSchema GetOrCreate(Type Type)
        {
            return Cache.GetOrAdd(Type, () => Create(Type));
        }

        #endregion
    }
}