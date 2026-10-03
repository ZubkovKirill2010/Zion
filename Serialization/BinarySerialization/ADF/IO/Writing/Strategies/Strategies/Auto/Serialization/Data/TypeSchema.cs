namespace Zion.Serialization.ADF
{
    public readonly struct TypeSchema<T>
    {
        #region Cache
        private static readonly Dictionary<Type, object> Cache = new(64);

        #endregion

        #region Data

        #endregion

        #region Constructors

        #endregion

        #region PublicMethods
        public static TypeSchema<T> GetOrCreate(ADFWritingContext Context, Type Type)
        {
            if (Cache.TryGetValue(Type, out object? Boxed))
            {
                return (TypeSchema<T>)Boxed;
            }
            var Created = TypeSchemaBuilder<T>.Create(Context, Type);
            Cache.Add(Type, Created);
            return Created;
        }

        #endregion
    }
}