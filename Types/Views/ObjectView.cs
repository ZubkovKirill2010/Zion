using System.Collections;

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
                int End = Range.End.IsFromEnd ? this.Length - Range.End.Value : Range.End.Value;

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


        protected virtual bool Equals(T A, T B)
        {
            return A.Equals(B);
        }


        public IEnumerable<T> For(int Start)
        {
            return ZEnumerable.Range(Start, Length).Select(Index => this[Index]);
        }

        public IEnumerable<T> For(int Start, int Count)
        {
            return ZEnumerable.For(Start, Count).Select(Index => this[Index]);
        }

        public IEnumerable<T> Range(int Start, int End)
        {
            return ZEnumerable.Range(Start, End).Select(Index => this[Index]);
        }

        public IEnumerable<T> Range(int Count)
        {
            return ZEnumerable.Range(Count).Select(Index => this[Index]);
        }


        public bool Begins(int Start, IEnumerable<T> Target, out int Count, Func<T, T, bool>? Equals = null)
        {
            ArgumentNullException.ThrowIfNull(Target);

            if (Start >= Length)
            {
                Count = 0;
                return false;
            }

            Equals ??= this.Equals;
            Count = 0;

            int Index = 0;

            foreach (T TargetItem in Target.Limit(Length - Start))
            {
                Count++;
                if (!Equals(this[Start + Index++], TargetItem))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Begins(int Start, IEnumerable<T> Target, Func<T, T, bool>? Equals = null)
        {
            ArgumentNullException.ThrowIfNull(Target);

            if (Start >= Length) { return false; }

            Equals ??= this.Equals;

            int Index = 0;

            foreach (T TargetItem in Target.Limit(Length - Start))
            {
                if (!Equals(this[Start + Index++], TargetItem))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Begins(int Start, ICollection<T> Target, Func<T, T, bool>? Equals = null)
        {
            return Begins(Start, Target, Target.Count, Equals);
        }

        public bool Begins(int Start, T[] Target, Func<T, T, bool>? Equals = null)
        {
            return Begins(Start, Target, Target.Length, Equals);
        }

        public bool Begins(int Start, ObjectView<T> Target, Func<T, T, bool>? Equals = null)
        {
            return Begins(Start, Target, Target.Length, Equals);
        }

        internal protected bool Begins(int Start, IEnumerable<T> Target, int Count, Func<T, T, bool>? Equals = null)
        {
            ArgumentNullException.ThrowIfNull(Target);

            if (Start >= Length || Start + Count >= Length)
            {
                return false;
            }

            Equals ??= this.Equals;

            int Index = 0;

            foreach (T TargetItem in Target)
            {
                if (!Equals(this[Start + Index++], TargetItem))
                {
                    return false;
                }
            }

            return true;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            return ZEnumerable.Range(Length).Select(Index => this[Index]).GetEnumerator();
        }
    }
}