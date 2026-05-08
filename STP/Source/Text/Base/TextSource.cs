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