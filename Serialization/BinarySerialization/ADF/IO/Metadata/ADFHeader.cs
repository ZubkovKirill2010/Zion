namespace Zion.Serialization.ADF
{
    public readonly struct ADFHeader : IBinarySerializable<ADFHeader>
    {
        public void Write(BinaryWriter Writer)
        {
            throw new NotImplementedException();
        }

        public static ADFHeader Read(BinaryReader Reader)
        {
            throw new NotImplementedException();
        }
    }
}