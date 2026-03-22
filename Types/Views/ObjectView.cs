using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace Zion
{
    public abstract class ObjectView<T> : IEnumerable<T>
    {
        public int Length
        {
            get;
            protected init
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(Length));
                field = value;
            }
        }

        public bool IsEmpty => Length == 0;


        public abstract T this[int Index]
        {
            get;
        }

        public T this[Index Index]
        {
            get
            {
                return Index.IsFromEnd ? this[Length - Index.Value] : this[Index.Value];
            }
        }

        public T[] this[Range Range]
        {
            get
            {
                int Start = Range.Start.IsFromEnd ? this.Length - Range.Start.Value : Range.Start.Value;
                int End   = Range.End.IsFromEnd   ? this.Length - Range.End.Value   : Range.End.Value;

                if (Start < 0 || Start > this.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(Range), $"Start index {Start} is out of range [0, {this.Length}]");
                }

                if (End < 0 || End > this.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(Range), $"End index {End} is out of range [0, {this.Length}]");
                }

                if (Start > End)
                {
                    throw new ArgumentException($"Start index {Start} must be less than or equal to end index {End}");
                }

                int Length = End - Start;

                T[] Result = new T[Length];

                for (int i = 0; i < Length; i++)
                {
                    Result[i] = this[Start + i];
                }

                return Result;
            }
        }
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            return ZEnumerable.Range(Length).Select(Index => this[Index]).GetEnumerator();
        }        
    }
}