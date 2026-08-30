namespace Zion.Serialization.ADF
{
    public sealed class StringIdRegistry : IWritableRegistry
    {
        private readonly Dictionary<string, uint> Data;
        private ushort LastId;

        public int NewItemsCount { get; private set; }

        public StringIdRegistry()
        {
            Data = new();
        }


        public uint GetOrAdd(string String)
        {
            if (Data.TryGetValue(String.NotNull(), out uint Id))
            {
                return Id;
            }
            Data.Add(String, LastId);
            return LastId++;
        }

        public bool TryGetId(string String, out uint Id)
        {
            return Data.TryGetValue(String, out Id);
        }
    }
}