namespace Zion.Serialization.ADF
{
    internal interface IWriteStrategy<T> : IWriteStrategy
    {
        Type IWriteStrategy.TargetType => typeof(T);

        void IWriteStrategy.Write(ADFWritingContext Context, StreamGroup Target, object Value)
        {
            if (Value is T Typed)
            {
                Write(Context, Target, Typed);
            }
            else
            {
                throw new InvalidCastException($"{Value.GetType()} can not cast in {TargetType}");
            }
        }

        public void Write(ADFWritingContext Context, StreamGroup Target, T Value);
    }
}