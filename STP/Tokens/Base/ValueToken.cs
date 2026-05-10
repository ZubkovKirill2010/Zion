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
    }
}