using System.Collections;

namespace Zion.STP
{
    public abstract class TextSource : IEnumerable<char>
    {
        public abstract char Current { get; }
        public abstract bool IsEnd { get; }

        public abstract TextSource BeginNew();

        public abstract void MoveNext();


        public bool Begins(string Target, out TextSource Readed)
        {
            Readed = this;
            if (IsEnd)
            {
                return false;
            }
            if (Target.NotNull().Length == 0)
            {
                return true;
            }
            if (Target.Length == 1)
            {
                if (Current == Target[0])
                {
                    Readed = BeginNew();
                    Readed.MoveNext();
                    return true;
                }

                return false;
            }

            TextSource Source = BeginNew();
            int Index = 0;

            while (!Source.IsEnd && Index < Target.Length)
            {
                if (Source.Current != Target[Index])
                {
                    return false;
                }

                Source.MoveNext();
                Index++;
            }

            if (Index == Target.Length)
            {
                Readed = Source;
                return true;
            }

            return false;
        }

        public bool Begins(string Target)
        {
            return Begins(Target, out _);
        }


        public void Skip(int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                if (IsEnd)
                {
                    break;
                }
                MoveNext();
            }
        }

        public void Skip(int Count, out int Skipped)
        {
            for (int i = 0; i < Count; i++)
            {
                if (IsEnd)
                {
                    Skipped = i;
                    break;
                }
                MoveNext();
            }
            Skipped = Count;
        }

        public void Skip(char Target)
        {
            while (!IsEnd && Current == Target)
            {
                MoveNext();
            }
        }

        public void Skip(char Target, out int Skipped)
        {
            Skipped = 0;

            while (!IsEnd && Current == Target)
            {
                Skipped++;
                MoveNext();
            }
        }

        public void Skip(Func<char, bool> Condition)
        {
            while (!IsEnd && Condition(Current))
            {
                MoveNext();
            }
        }

        public void Skip(Func<char, bool> Condition, out int Skipped)
        {
            Skipped = 0;

            while (!IsEnd && Condition(Current))
            {
                Skipped++;
                MoveNext();
            }
        }


        public int CountOf(Func<char, bool> Condition, out TextSource Readed)
        {
            if (IsEnd)
            {
                Readed = this;
                return 0;
            }

            TextSource Source = BeginNew();

            int Count = 0;

            while (!Source.IsEnd && Condition(Source.Current))
            {
                Count++;
                Source.MoveNext();
            }

            Readed = Source;
            return Count;
        }

        public int CountOf(Func<char, bool> Condition)
        {
            return CountOf(Condition, out _);
        }

        public int CountOf(char Target, out TextSource Readed)
        {
            return CountOf(Char => Char == Target, out Readed);
        }

        public int CountOf(char Target)
        {
            return CountOf(Target, out _);
        }


        public bool TryRead(int Length, out string String, out TextSource Readed)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Length);

            String = string.Empty;
            Readed = this;

            if (Length == 0)
            {
                return true;
            }

            if (IsEnd)
            {
                return false;
            }

            TextSource Source = BeginNew();
            List<char> Chars = new List<char>(Length);

            while (!Source.IsEnd && Chars.Count < Length)
            {
                Chars.Add(Source.Current);
                Source.MoveNext();
            }

            if (Chars.Count == Length)
            {
                Readed = Source;
                String = string.FromCharCollection(Chars);
                return true;
            }

            return false;
        }


        public bool CurrentIs(char Target)
        {
            return !IsEnd && Current == Target;
        }

        public bool CurrentIs(Func<char, bool> Condition)
        {
            return !IsEnd && Condition(Current);
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<char> GetEnumerator()
        {
            while (!IsEnd)
            {
                yield return Current;
                MoveNext();
            }
        }
    }
}