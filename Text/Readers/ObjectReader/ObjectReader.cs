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
            if (Reader.TryRead(this, Text, Index, out Value, out int Length))
            {
                Index += Length;
                return true;
            }
            return false;
        }

        public bool TryRead<T>(out T Value) where T : IReadable<T>
        {
            if (T.TryRead(this, Text, Index, out Value, out int Length))
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

        public T Read<T>() where T : IReadable<T> 
        {
            return Unsafe((out T Value) => TryRead(out Value));
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


        public bool TryRead(string Target)
        {
            if (Text.Begins(Index, Target))
            {
                return true;
            }
            return false;
        }

        public bool TryRead(TextView Target)
        {
            if (Text.Begins(Index, Target))
            {
                return true;
            }
            return false;
        }

        public bool TryRead(params char[] Target)
        {
            if (Text.Begins(Index, Target))
            {
                return true;
            }
            return false;
        }

        public bool TryRead(params IEnumerable<char> Target)
        {
            if (Text.Begins(Index, Target))
            {
                return true;
            }
            return false;
        }

        public void Read(string Target)
        {
            if (!TryRead(Target))
            {
                throw new FormatException($"Failed to read \"{Target}\" at position {Index}.");
            }
        }

        public void Read(TextView Target)
        {
            if (!TryRead(Target))
            {
                throw new FormatException($"Failed to read \"{Target.ToString()}\" at position {Index}.");
            }
        }

        public void Read(params char[] Target)
        {
            if (!TryRead(Target))
            {
                throw new FormatException($"Failed to read \"{new string(Target)}\" at position {Index}.");
            }
        }

        public void Read(params IEnumerable<char> Target)
        {
            if (!TryRead(Target))
            {
                throw new FormatException($"Failed to read 'Enumerable' at position {Index}.");
            }
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