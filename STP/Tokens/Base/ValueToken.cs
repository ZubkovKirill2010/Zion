namespace Zion.STP
{
    public abstract class ValueToken<T> : Token
    {
        public T? Value { get; init; }

        public override string ToString()
        {
            string ValueString = Value is null
                ? "null"
                : Value.ToString() ?? "NullSring";

            return $"[{GetType().Name.RemoveSuffix("Token")}:{ValueString}]";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Length, Value);
        }

        protected override bool DataEquals(Token Other)
        {
            return Other is ValueToken<T> ValueToken && EqualityComparer<T?>.Default.Equals(Value, ValueToken.Value);
        }
    }
}