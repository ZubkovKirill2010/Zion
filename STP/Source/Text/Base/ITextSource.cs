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

        public bool Begins(string Target)
        {
            if (Target.NotNull().Length == 0) { return true; }

            ITextSource Part = BeginNew();
            int Index = 0;

            while (!Part.IsEnd && Index < Target.Length)
            {
                if (Part.Current != Target[Index])
                {
                    return false;
                }

                Part.MoveNext();
                Index++;
            }

            return Index == Target.Length;
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