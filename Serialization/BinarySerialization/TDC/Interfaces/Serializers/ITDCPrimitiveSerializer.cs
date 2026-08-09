namespace Zion.Serialization.TDC
{
    public interface ITDCPrimitiveSerializer<T>
    {
        public void Write(PrimitiveTDCWriter Writer, T Value);

        public static abstract T Read(PrimitiveTDCReader Reader);
    }
}