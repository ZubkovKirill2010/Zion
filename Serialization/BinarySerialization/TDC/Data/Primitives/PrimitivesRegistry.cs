namespace Zion.Serialization.TDC
{
    public sealed class PrimitivesRegistry : IBinarySerializable<PrimitivesRegistry>
    {
        private const int SimplePrimitiveCount = 64;

        private readonly List<PrimitiveFormat> Data;

        public int Count => Data.Count;


        public PrimitivesRegistry() : this(32) { }
        
        public PrimitivesRegistry(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Data = new(Capacity);
        }

        private PrimitivesRegistry(List<PrimitiveFormat> Data)
        {
            this.Data = Data.NotNull();
        }


        public PrimitiveFormat this[ushort Id]
        {
            get
            {
                int Index = Id - SimplePrimitiveCount;
                if (Index < 0 || Index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(Id), $"Index out of range [{SimplePrimitiveCount}..{Count - SimplePrimitiveCount})");
                }
                return Data[Index];
            }
        }


        public void Add(PrimitiveFormat Format)
        {
            Data.Add(Format);
        }

        public void Add(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();
            Data.EnsureCapacity(this.Count + Count);
            for (int i = 0; i < Count; i++)
            {
                Data.Add
                (
                    Reader.Read<PrimitiveFormat>()
                );
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.WriteCollection(Data);
        }

        public static PrimitivesRegistry Read(BinaryReader Reader)
        {
            return new PrimitivesRegistry
            (
                Reader.ReadList<PrimitiveFormat>()
            );
        }        
    }
}