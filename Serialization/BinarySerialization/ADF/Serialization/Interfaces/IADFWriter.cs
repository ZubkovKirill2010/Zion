namespace Zion.Serialization.ADF
{
    public interface IADFWriter<T>
    {
        public void Write(ADFObjectWriter Writer, T Value);
    }
}