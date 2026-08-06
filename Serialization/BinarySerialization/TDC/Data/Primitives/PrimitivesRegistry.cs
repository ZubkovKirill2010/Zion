namespace Zion.Serialization.TDC
{
    public sealed class PrimitivesRegistry : IBinarySerializable<PrimitivesRegistry>
    {
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
                int Index = Id;
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
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