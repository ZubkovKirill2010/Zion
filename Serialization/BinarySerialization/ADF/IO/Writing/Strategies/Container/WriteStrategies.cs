namespace Zion.Serialization.ADF
{
    public sealed class WriteStrategies
    {
        private readonly Dictionary<Type, WriteEntry> Strategies = new();


        internal void Add<T>(Type Type, uint FormatId, IWriteStrategy<T> Strategy)
        {
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException();
            }
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
    }
}