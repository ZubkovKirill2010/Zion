namespace Zion.Serialization.TDC
{
    public readonly struct ContainerFormat : IBinarySerializable<ContainerFormat>
    {
        public ContainerFormat()
        {

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