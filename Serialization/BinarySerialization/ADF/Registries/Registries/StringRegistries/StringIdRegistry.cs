namespace Zion.Serialization.ADF
{
    public sealed class StringIdRegistry : IWritableRegistry
    {
        public const uint Null = 0u;

        private readonly Dictionary<string, uint> Data;
        private uint LastId = 1;

        public bool IsChanged { get; private set; }


        public StringIdRegistry()
        {
            Data = new();
        }


        public void Write(ADFObjectWriter Writer)
        {
            //TODO: IWritableRegistry.Write
        }


        public uint GetOrAdd(string? String)
        {
            if (String is null)
            {
                return Null;
            }
            if (Data.TryGetValue(String, out uint Id))
            {
                return Id;
            }
            Data.Add(String, LastId);
            return LastId++;
        }

        public bool TryGetId(string? String, out uint Id)
        {
            if (String is null)
            {
                Id = Null;
                return true;
            }
            return Data.TryGetValue(String, out Id);
        }


        public string? GetString(in uint Id)
        {
            if (Id == Null)
            {
                return null;
            }

            if (Id >= LastId)
            {
                throw new KeyNotFoundException($"String with id '{Id}' not exists");
            }

            foreach (var Pair in Data)
            {
                if (Pair.Value == Id)
                {
                    return Pair.Key;
                }
            }

            throw new KeyNotFoundException($"String with id '{Id}' not exists");
        }
    }
}