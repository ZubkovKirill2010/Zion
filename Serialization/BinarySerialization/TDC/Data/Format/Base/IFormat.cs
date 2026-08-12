namespace Zion.Serialization.TDC
{
    public interface IFormat : IBinaryWritable
    {
        public byte FormatId { get; }
    }
}