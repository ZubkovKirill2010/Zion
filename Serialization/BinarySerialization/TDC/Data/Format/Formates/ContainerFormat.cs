namespace Zion.Serialization.TDC
{
    public readonly struct ContainerFormat : IFormat<ContainerFormat>
    {
        public byte FormatId { get; } = 1;


        public ContainerFormat() : this(16) { }

        public ContainerFormat(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            //TODO
        }


        public bool Contains(string Name)
        {
            throw new NotImplementedException();
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