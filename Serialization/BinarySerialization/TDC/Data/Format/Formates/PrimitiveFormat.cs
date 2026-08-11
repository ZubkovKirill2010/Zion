namespace Zion.Serialization.TDC
{
    public readonly struct PrimitiveFormat : IFormat<PrimitiveFormat>
    {
        public PrimitiveFormat()
        {

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