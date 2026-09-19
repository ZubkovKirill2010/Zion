namespace Zion.Serialization.ADF
{
    internal interface IWriteStrategy<T> : IWriteStrategy
    {
        Type IWriteStrategy.TargetType => typeof(T);

        void IWriteStrategy.Write(ArenaStream Stream, object Value)
        {
            if (Value is T Typed)
            {
                Write(Stream, Typed);
            }
            else
            {
                throw new InvalidCastException($"{Value.GetType()} can not cast in {TargetType}");
            }
        }

        public void Write(ArenaStream Stream, T Value);
    }
}