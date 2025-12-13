using System.Collections;

namespace Zion.STP
{
    public sealed class StringView : IList<char>
    {
        private readonly GapBuffer<char> Buffer;

        public int Length => Buffer.Count;
        public int Count => Buffer.Count;

        public bool IsReadOnly => Buffer.IsReadOnly;

        public char this[int Index]
        {
            get => Buffer[Index];
            set => Buffer[Index] = value;
        }

        public event Action<int>? Changed;

        public StringView()
            : this(200) { }
        public StringView(int Capacity)
        {
            Buffer = new GapBuffer<char>(Capacity);
        }


        public void Add(char Char)
        {
            int Length = Buffer.Count;
            Buffer.Add(Char);
            Changed?.Invoke(Length);
        }

        public void Add(string String)
        {
            int Length = Buffer.Count;
            Buffer.Add(String);
            Changed?.Invoke(Length);
        }

        public void Insert(int Index, char Char)
        {
            Buffer.Insert(Index, Char);
            Changed?.Invoke(Index);
        }

        public void Insert(int Index, string String)
        {
            Buffer.Insert(Index, String);
            Changed?.Invoke(Index);
        }

        public void RemoveAt(int Index)
        {
            Buffer.RemoveAt(Index);
            Changed?.Invoke(Index);
        }

        public int IndexOf(char Char)
        {
            return Buffer.IndexOf(Char);
        }

        public void Clear()
        {
            Buffer.Clear();
        }

        public bool Contains(char Item)
        {
            return Buffer.Contains(Item);
        }

        public void CopyTo(char[] Array, int ArrayIndex)
        {
            Buffer.CopyTo(Array, ArrayIndex);
        }

        public bool Remove(char Item)
        {
            return Buffer.Remove(Item);
        }


        public bool Begins(int Index, string Target, bool IgnoreCase = false)
        {
            if (Index >= Length)
            {
                throw new ArgumentException($"Index(={Index}) >= Length(={Length})");
            }
            if (Length - Index < Target.Length)
            {
                return false;
            }

            if (IgnoreCase)
            {
                for (int i = 0; i < Target.Length; i++)
                {
                    if (char.ToLower(this[Index + i]) != char.ToLower(Target[i]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Target.Length; i++)
                {
                    if (this[Index + i] != Target[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool Exists(string Target, out int StartIndex)
        {
            ArgumentNullException.ThrowIfNull(Target);

            StartIndex = -1;

            if (Length == 0 && Target.Length == 0)
            {
                return true;
            }
            if (Target.Length > Length)
            {
                return false;
            }

            StartIndex = 0;
            int End = Length - Target.Length;
            bool Founded = true;

            while (StartIndex <= End)
            {
                for (int j = 0; j < Target.Length; j++)
                {
                    if (this[StartIndex + j] != Target[j])
                    {
                        Founded = false;
                        break;
                    }
                }

                if (Founded)
                {
                    return true;
                }

                StartIndex++;
            }

            StartIndex = -1;
            return false;
        }

        public int IndexOf(Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(Condition);

            for (int i = 0; i < Length; i++)
            {
                if (Condition(this[i]))
                {
                    return i;
                }
            }
            return -1;
        }


        public bool IsIdentifier()
        {
            if (Length == 0 || char.IsNumber(this[0]))
            {
                return false;
            }

            for (int i = 1; i < Length; i++)
            {
                char Char = this[i];

                if (!(Char.IsEnglish() || Char == '_' || char.IsDigit(Char)))
                {
                    return false;
                }
            }

            return true;
        }

        public int SkipSpaces(int Start)
        {
            return Skip(Start, char.IsWhiteSpace);
        }

        public int Skip(int Start, Predicate<char> Condition)
        {
            int Index = Start;
            while (Index < Length && Condition(this[Index]))
            {
                Index++;
            }
            return Index;
        }

        public int Skip(int Start, params IEnumerable<char> SkipChars)
        {
            int Index = Start;
            while (Index < Length && SkipChars.Contains(this[Index]))
            {
                Index++;
            }
            return Index;
        }


        public IEnumerable<char> Range(int Start, int Length)
        {
            int Index = Start;
            int End = Start + Length;

            while (Index < End)
            {
                yield return this[Index++];
            }
        }

        public IEnumerator<char> GetEnumerator()
        {
            return Buffer.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}