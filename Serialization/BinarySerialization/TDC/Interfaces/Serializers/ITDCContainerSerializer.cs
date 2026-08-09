namespace Zion.Serialization.TDC
{
    public interface ITDCContainerSerializer<T>
    {
        public void Write(ContainerTDCWriter Writer, T Value);

        public static abstract T Read(ContainerTDCReader Reader);
    }
}