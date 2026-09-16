namespace Zion.Serialization.ADF
{
    public interface IADFSerializer<T>
    {
        public void Write(ADFObjectWriter Writer, T Value);
    }
}