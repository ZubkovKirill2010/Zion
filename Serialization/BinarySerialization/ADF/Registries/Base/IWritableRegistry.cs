namespace Zion.Serialization.ADF
{
    public interface IWritableRegistry : IADFWritable
    {
        public int NewItemsCount { get; }
    }
}