namespace Zion.Serialization.TDC
{
    public interface ITDCContainer<T> where T : ITDCContainer<T>
    {
        public void Write(TDCWriter Writer);

        public static abstract T Read(TDCReader Reader);
    }
}