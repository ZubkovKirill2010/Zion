namespace Zion.Serialization.TDC
{
    public interface ITDCContainer<T>
    {
        public void Write(ContainerTDCWriter Writer);

        public static abstract T Read(ContainerTDCWriter Reader);
    }
}