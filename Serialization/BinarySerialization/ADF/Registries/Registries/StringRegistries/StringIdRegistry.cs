namespace Zion.Serialization.ADF
{
    public sealed class StringIdRegistry : IWritableRegistry
    {
        private readonly Dictionary<string, uint> Data;
        private uint LastId = 1;

        public int NewItemsCount { get; private set; }

        public StringIdRegistry()
        {
            Data = new();
        }


        public uint GetOrAdd(string String)
        {
            if (String is null)
            {
                return 0u;
            }
            if (Data.TryGetValue(String, out uint Id))
            {
                return Id;
            }
            Data.Add(String, LastId);
            return LastId++;
        }

        public bool TryGetId(string String, out uint Id)
        {
            if (String is null)
            {
                Id = 0u;
                return true;
            }
            return Data.TryGetValue(String, out Id);
        }
    }
}