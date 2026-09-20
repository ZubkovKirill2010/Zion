namespace Zion.Serialization.ADF
{
    internal interface IWriteStrategy<T> : IWriteStrategy
    {
        Type IWriteStrategy.TargetType => typeof(T);

        void IWriteStrategy.Write(StreamGroup Group, object Value)
        {
            if (Value is T Typed)
            {
                Write(Group, Typed);
            }
            else
            {
                throw new InvalidCastException($"{Value.GetType()} can not cast in {TargetType}");
            }
        }

        public void Write(StreamGroup Group, T Value);
    }
}