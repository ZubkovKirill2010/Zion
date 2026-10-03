namespace Zion.Serialization.ADF
{
    internal interface IWriteStrategy
    {
        public Type TargetType { get; }

        public void Write(ADFWritingContext Context, StreamGroup Target, object Value);
    }
}