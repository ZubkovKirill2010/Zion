namespace Zion.Serialization.TDC
{
    public readonly struct PrimitiveFormat : IBinarySerializable<PrimitiveFormat>
    {
        //Type sequence

        public PrimitiveFormat()
        {

        }


        public void Write(BinaryWriter Writer)
        {

        }

        public static PrimitiveFormat Read(BinaryReader Reader)
        {
            return default!;
        }
    }
}