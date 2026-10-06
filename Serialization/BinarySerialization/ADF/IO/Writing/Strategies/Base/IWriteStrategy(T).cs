namespace Zion.Serialization.ADF
{
    internal interface IWriteStrategy<in T> : IWriteStrategy
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

        public void Write(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            if (WriteStrategyHelper<T>.Setup(Context, Value, ref Target))
            {
                return;
            }
            WriteData(Context, Target, Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value);
    }
}