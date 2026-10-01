namespace Zion.Serialization.ADF
{
    public readonly struct TypeSchema<T>
    {
        #region Cache
        private static readonly Dictionary<Type, object> Cache = new(64);

        #endregion

        #region Data
        private readonly AutoWriter<T> Writer;
        private readonly HashSet<string> Names;

        #endregion

        #region Constructors
        internal TypeSchema(AutoWriter<T> Writer, HashSet<string> Names)
        {
            this.Writer = Writer.NotNull();
            this.Names = Names.NotNull();
        }

        #endregion

        #region PublicMethods
        public static TypeSchema<T> GetOrCreate(ADFWritingContext Context, Type Type)
        {
            if (Cache.TryGetValue(Type, out object? Boxed))
            {
                return (TypeSchema<T>)Boxed;
            }
            var Created = TypeSchemaBuilder<T>.Create(Context, Type, out var Format);//TODO: Replace Format
            Cache.Add(Type, Created);
            return Created;
        }


        public void Write(StreamGroup Target, T Value)
        {
            Writer.Invoke(Target, Value);
        }

        public bool ContainsName(string Name)
        {
            return Names.Contains(Name);
        }

        #endregion
    }
}