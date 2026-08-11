namespace Zion.Serialization.TDC
{
    public sealed class DataRegistry
    {
        private readonly Dictionary<string, DataPosition> Data;
        private readonly List<(string, DataPosition)> Page;

        public int Count => Data.Count;


        public DataRegistry() : this(32) { }

        public DataRegistry(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Data = new(Capacity);
            Page = new(Capacity);
        }


        public bool Contains(string Name)
        {
            return Data.ContainsKey(Name);
        }

        public bool TryGetPosition(string Name, out DataPosition Position)
        {
            return Data.TryGetValue(Name, out Position);
        }

        
        public void Add(string Name, DataPosition Position)
        {
            if (!Data.TryAdd(Name, Position))
            {
                throw new ArgumentException($"Parameter with name '{Name}' already exists");
            }

            Page?.Add((Name, Position));
        }


        public void Write(BinaryWriter Writer)
        {
            var Data = this.Page;

            Writer.Write(Data.Count);

            foreach (var Pair in Data)
            {
                Writer.Write(Pair.Item1);
                Writer.Write(Pair.Item2);
            }

            Data.Clear();
        }

        public void Read(BinaryReader Reader)
        {
            var Data = this.Data;
            int Count = Reader.ReadInt32();

            Data.EnsureCapacity(Data.Count + Count);

            for (int i = 0; i < Count; i++)
            {
                Data.Add
                (
                    Reader.ReadString(),
                    Reader.Read<DataPosition>()
                );                
            }
        }
    }
}