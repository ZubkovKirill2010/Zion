namespace Zion.Serialization.TDC
{
    public readonly struct ContainerFormat : IBinarySerializable<ContainerFormat>
    {
        private readonly Dictionary<string, DataDefinition> Data;

        public ContainerFormat() : this(16) { }

        public ContainerFormat(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Data = new(Capacity);
        }

        
        public bool Contains(string Name)
        {
            return Data.ContainsKey(Name);
        }


        public void Add(string Name, ushort TypeId)
        {
            Add(Name, new DataDefinition((ushort)Data.Count, TypeId));
        }

        public void Add(string Name, DataDefinition Definition)
        {
            Data.Add(Name.NotNull(), Definition);
        }


        public void Write(BinaryWriter Writer)
        {
            throw new NotImplementedException();
        }

        public static ContainerFormat Read(BinaryReader Reader)
        {
            throw new NotImplementedException();
        }
    }
}