namespace Zion.Serialization.ADF
{
    public interface IWritableRegistry : IADFWritable
    {
        public bool IsChanged { get; }
    }
}