namespace Zion.Serialization.TDC
{
    public sealed class ContainersRegistry : IBinarySerializable<ContainersRegistry>
    {
        private readonly List<ContainerFormat> Data;


        private ContainersRegistry(List<ContainerFormat> Data)
        {
            this.Data = Data.NotNull();
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.WriteCollection(Data);
        }

        public static ContainersRegistry Read(BinaryReader Reader)
        {
            return new ContainersRegistry(Reader.ReadList<ContainerFormat>());
        }        
    }
}