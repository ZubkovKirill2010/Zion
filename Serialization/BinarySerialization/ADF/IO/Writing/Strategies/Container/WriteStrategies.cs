namespace Zion.Serialization.ADF
{
    public sealed class WriteStrategies
    {
        private readonly Dictionary<Type, WriteEntry> Strategies = new();


        internal WriteEntry<T> Add<T>(Type Type, WriteEntry<T> Entry)
        {
            ThrowIfNotAssignable<T>(Type);
            Strategies.Add(Type, Entry);
            return Entry;
        }

        internal void Add<T>(Type Type, uint FormatId, IWriteStrategy<T> Strategy)
        {
            ThrowIfNotAssignable<T>(Type);
            Strategies.Add(Type, new(FormatId, Strategy));
        }

        internal bool TryGetEntry<T>(Type Type, out WriteEntry<T> Entry)
        {
            if (Strategies.TryGetValue(Type, out var Generalized))
            {
                Entry = Generalized;
                return true;
            }

            Entry = default!;
            return false;
        }

        internal WriteEntry<T> GetEntry<T>(Type Type)
        {
            return (WriteEntry<T>)Strategies[Type];
        }


        private static void ThrowIfNotAssignable<T>(Type Type)
        {
            if (!Type.IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException($"{Type} is not assignable from {typeof(T)}");
            }
        }
    }
}