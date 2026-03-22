namespace Zion
{
    public sealed class ObjectReader
    {
        private readonly TextView Text;

        public int Index { get; private set; }

        public ObjectReader(TextView Text)
        {
            this.Text = Text.NotNull();
        }

        public ObjectReader(string Text)
        {
            this.Text = new StringView(Text.NotNull());
        }


        private T Unsafe<T>(SafeGetter<T> SafeFunction)
        {
            if (SafeFunction(out T Value))
            {
                return Value;
            }

            throw new FormatException($"Failed to read value of type '{typeof(T).Name}' at position {Index}. Context: ...{Text[]}... (^ at position {positionInContext})");
        }
    }
}