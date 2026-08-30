namespace Zion.Serialization.ADF
{
    public sealed class DataRegistry : IRegistry
    {
        private readonly StringIdRegistry StringRegistry;
        private readonly Dictionary<uint, DataDefinition> Parameters;
        private readonly List<(uint, DataDefinition)> NewItems;

        public int NewItemsCount { get; private set; }


        public DataRegistry(StringIdRegistry StringRegistry)
        {
            this.StringRegistry = StringRegistry.NotNull();
            Parameters = new();
            NewItems   = new();
        }


        public void Add(string Name, DataDefinition Definition)
        {
            if (!TryAdd(Name, Definition))
            {
                throw new ADFRepeatedNameException(Name);
            }
        }

        public bool TryAdd(string Name, DataDefinition Definition)
        {
            uint NameId = StringRegistry.GetOrAdd(Name);
            if (!Parameters.TryAdd(NameId, Definition))
            {
                return false;
            }
            NewItems.Add((NameId, Definition));
            return true;
        }

        public bool Contains(string Name)
        {
            return StringRegistry.TryGetId(Name, out uint Id)
                && Parameters.ContainsKey(Id);
        }

        public bool TryGetDefinition(string Name, out DataDefinition Definition)
        {
            if (StringRegistry.TryGetId(Name, out uint Id)
                && Parameters.TryGetValue(Id, out Definition))
            {
                return true;
            }

            Definition = default;
            return false;
        }
    }
}