namespace Zion.Serialization.TDC
{
    public interface ITDCPrimitive<T> where T : ITDCPrimitive<T>
    {
        public void Write();

        public static abstract T Read();
    }
}