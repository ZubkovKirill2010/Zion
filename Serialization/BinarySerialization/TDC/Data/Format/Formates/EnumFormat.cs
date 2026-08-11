namespace Zion.Serialization.TDC
{
    public readonly struct EnumFormat : IFormat<EnumFormat>
    {
        public readonly int Length;


        public EnumFormat()
        {

        }


        public void Write(BinaryWriter Writer)
        {
            throw new NotImplementedException();
        }

        public static EnumFormat Read(BinaryReader Reader)
        {
            throw new NotImplementedException();
        }
    }
}