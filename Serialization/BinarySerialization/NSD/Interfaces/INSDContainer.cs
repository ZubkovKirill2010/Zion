namespace Zion.Serialization.NSD
{
    public interface INSDContainer<T> where T : INSDContainer<T>
    {
        public void Write(NSDWriteContext Context);

        public static abstract T Read(NSDReadContext Context);
    }
}