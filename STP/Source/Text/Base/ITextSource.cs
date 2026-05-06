using System.Collections;

namespace Zion.STP
{
    public interface ITextSource : IEnumerable<char>
    {
        public char Current { get; }
        public bool IsEnd { get; }

        public bool MoveNext();

        public ITextSource BeginNew();

        public void Reset();


        public bool Begins(string Target, out ITextSource Readed)
        {
            Readed = this;
            if (Target.NotNull().Length == 0) { return true; }

            ITextSource Source = BeginNew();
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


        public bool CurrentIs(char Target)
        {
            return !IsEnd && Current == Target;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            while (!IsEnd)
            {
                yield return Current;
                if (!MoveNext()) { break; }
            }
        }
    }
}