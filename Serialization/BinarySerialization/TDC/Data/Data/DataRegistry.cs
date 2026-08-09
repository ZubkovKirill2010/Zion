namespace Zion.Serialization.TDC
{
    public sealed class DataRegistry : IBinarySerializable<DataRegistry>
    {
        private readonly Dictionary<string, DataPosition> Data;

        private List<(string, DataPosition)>? Page { get; init; }
        public bool PendingData
        {
            get  => Page is not null;
            init => Page = value ? new(32) : null;
        }

        public int Count => Data.Count;


        public DataRegistry() : this(16) { }

        public DataRegistry(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Data = new(Capacity);
        }

        private DataRegistry(Dictionary<string, DataPosition> Data)
        {
            this.Data = Data.NotNull();
        }


        public void ResetPage()
        {
            Page?.Clear();
        }


        public bool Contains(string Key)
        {
            return Data.ContainsKey(Key);
        }

        public bool TryGetPosition(string Name, out DataPosition Position)
        {
            return Data.TryGetValue(Name, out Position);
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Data.Count);
            foreach (var Item in Data)
            {
                Writer.Write(Item.Key);
                Writer.Write(Item.Value);
            }
        }

        public static DataRegistry Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();
            Dictionary<string, DataPosition> Data = new(Count);

            for (int i = 0; i < Count; i++)
            {
                Data.Add
                (
                    Reader.ReadString(),
                    Reader.Read<DataPosition>()
                );
            }

            return new(Data);
        }        
    }
}