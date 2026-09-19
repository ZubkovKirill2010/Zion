namespace Zion.Serialization.ADF
{
    internal interface IWriteStrategy
    {
        public Type TargetType { get; }

        public void Write(ArenaStream Stream, object Value);
    }
}