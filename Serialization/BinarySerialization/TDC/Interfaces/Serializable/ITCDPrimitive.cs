namespace Zion.Serialization.TDC
{
    public interface ITDCPrimitive<T> where T : ITDCPrimitive<T>
    {
        public void Write(PrimitiveTDCWriter Writer);

        public static abstract T Read(PrimitiveTDCReader Reader);
    }
}