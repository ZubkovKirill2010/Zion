namespace Zion.Serialization.ADF
{
    public interface IADFSerializer
    {
        public Type TargetType { get; }

        public void Write(ADFObjectWriter Writer, object Value);
    }
}