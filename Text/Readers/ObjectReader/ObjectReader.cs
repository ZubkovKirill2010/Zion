namespace Zion
{
    public sealed partial class ObjectReader //_Main
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


        public bool TryRead<T>(IReader<T> Reader, out T Value)
        {
            if (Reader.TryRead(Text, out Value, out int Length))
            {
                Index += Length;
                return true;
            }
            return false;
        }

        public T Read<T>(IReader<T> Reader)
        {
            return Unsafe((out T Value) => TryRead(Reader, out Value));
        }


        public void SetIndex(int NewIndex)
        {
            ArgumentOutOfRangeException.ThrowIf
            (
                int.IsNegative(NewIndex) || NewIndex >= Text.Length,
                $"Index(={Index}) < 0 || >= Text.Length(={Text.Length})"
            );
            Index = NewIndex;
        }


        public void Skip(int Count)
        {
            Index += Count;
        }

        public void Skip(Func<char, bool> Condition)
        {
            ArgumentNullException.ThrowIfNull(Condition);

            while (Index < Text.Length && Condition(Text[Index]))
            {
                Index++;
            }
        }

        public void SkipSpaces()
        {
            Skip(char.IsWhiteSpace);
        }


        private T Unsafe<T>(SafeGetter<T> SafeFunction)
        {
            return SafeFunction(out T Value)
                ? Value
                : throw new FormatException($"Failed to read value of type '{typeof(T).Name}' at position {Index}.");
        }
    }
}