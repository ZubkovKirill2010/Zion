namespace Zion.Serialization.ADF
{
    public sealed class StringIdRegistry : IWritableRegistry
    {
        private readonly Dictionary<string, ushort> Data;
        private ushort LastId;

        public int NewItemsCount { get; private set; }

        public StringIdRegistry()
        {
            Data = new();
        }


        public ushort GetOrAdd(string String)
        {
            if (Data.TryGetValue(String.NotNull(), out ushort Id))
            {
                return Id;
            }
            Data.Add(String, LastId);
            return LastId++;
        }
    }
}