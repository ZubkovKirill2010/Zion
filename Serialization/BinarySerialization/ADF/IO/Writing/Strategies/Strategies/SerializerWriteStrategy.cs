namespace Zion.Serialization.ADF
{
    internal sealed class SerializerWriteStrategy<T> : IWriteStrategy<T>
    {
        public void Write(ArenaStream Stream, T Value)
        {
            throw new NotImplementedException();
        }
    }
}