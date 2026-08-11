namespace Zion.Serialization.TDC
{
    //TODO
    public readonly struct PrimitiveFormat : IFormat<PrimitiveFormat>
    {
        public PrimitiveFormat(List<ushort> Sequence)
        {
            
        }


        public ushort this[int Index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public void Write(BinaryWriter Writer)
        {
            throw new NotImplementedException();
        }

        public static PrimitiveFormat Read(BinaryReader Reader)
        {
            throw new NotImplementedException();
        }
    }
}