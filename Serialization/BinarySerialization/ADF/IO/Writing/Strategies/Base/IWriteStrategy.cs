namespace Zion.Serialization.ADF
{
    internal interface IWriteStrategy
    {
        public Type TargetType { get; }

        public void Write(StreamGroup Group, object Value);
    }
}