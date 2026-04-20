namespace Zion
{
    public sealed partial class ObjectReader //_Main
    {
        public readonly TextView Text;
        public readonly int Length;

        public int Index
        {
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIf
                (
                    int.IsNegative(value) || value > Length,
                    $"Index(={Index}) < 0 || > Length(={Length})"
                );

                if (IgnoreWhiteSpaces)
                {
                    while (value < Length && char.IsWhiteSpace(Text[value]))
                    {
                        value++;
                    }
                }

                field = value;
            }
        }

        public bool IgnoreWhiteSpaces { get; init; }

        public bool IsEnd => Index == Length;


        public ObjectReader(TextView Text)
        {
            this.Text = Text.NotNull();
            Length = Text.Length;
        }

        public ObjectReader(string Text)
            : this(new StringView(Text.NotNull())) { }


        public bool TryRead<T>(IReader<T> Reader, out T Value)
        {
            if (Reader.TryRead(this, Index, out Value, out int Length))
            {
                Index += Length;
                return true;
            }
            return false;
        }

        public bool TryRead<T>(out T Value) where T : IReadable<T>
        {
            if (T.TryRead(this, Index, out Value, out int Length))
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


        public bool TryRead(char Target)
        {
            if (!IsEnd && Text[Index] == Target)
            {
                Index++;
                return true;
            }

            return false;
        }

        public bool TryRead(string Target)
        {
            if (Text.Begins(Index, Target))
            {
                Index += Target.Length;
                return true;
            }
            return false;
        }

        public bool TryRead(TextView Target)
        {
            if (Text.Begins(Index, Target))
            {
                Index += Target.Length;
                return true;
            }
            return false;
        }

        public bool TryRead(params char[] Target)
        {
            if (Text.Begins(Index, Target))
            {
                Index += Target.Length;
                return true;
            }
            return false;
        }

        public bool TryRead(params IEnumerable<char> Target)
        {
            if (Text.Begins(Index, Target, out int Length))
            {
                Index += Length;
                return true;
            }
            return false;
        }


        public void Read(char Target)
        {
            if (!TryRead(Target))
            {
                throw new FormatException($"Failed to read \"{Target}\" at position {Index}.");
            }
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

        public void Skip(char Target)
        {
            while (Index < Length && Text[Index] == Target)
            {
                Index++;
            }
        }

        public void Skip(params IEnumerable<char> Targets)
        {
            while (Index < Length && Targets.Contains(Text[Index]))
            {
                Index++;
            }
        }

        public void Skip(HashSet<char> Targets)
        {
            while (Index < Length && Targets.Contains(Text[Index]))
            {
                Index++;
            }
        }

        public void Skip(Func<char, bool> Condition)
        {
            ArgumentNullException.ThrowIfNull(Condition);

            while (Index < Length && Condition(Text[Index]))
            {
                Index++;
            }
        }

        public void SkipSpaces()
        {
            if (!IgnoreWhiteSpaces)
            {
                Skip(char.IsWhiteSpace);
            }
        }

        public void SkipTo(char Target)
        {
            while (Index < Length && Text[Index] != Target)
            {
                Index++;
            }
        }


        public bool IsWithout(int Index)
        {
            return int.IsNegative(Index) || Index >= Length;
        }


        private bool IsAt(int Index, char Target)
        {
            return int.IsPositive(Index) && Index < Length && Text[Index] == Target;
        }

        private bool IsNotAt(int Index, char Exception)
        {
            return int.IsPositive(Index) && Index < Length && Text[Index] != Exception;
        }

        private T Unsafe<T>(SafeGetter<T> SafeFunction)
        {
            return SafeFunction(out T Value)
                ? Value
                : throw new FormatException($"Failed to read value of type '{typeof(T).Name}' at position {Index}.");
        }
    }
}