namespace Zion
{
    public readonly struct Range<T> : IRange<T>, IEquatable<Range<T>> where T : IComparable<T>
    {
        public T Start { get; }
        public T End { get; }

        public Range(T Start, T End)
        {
            if (Start.CompareTo(End) > 0)
            {
                throw new ArgumentException("Start must be <= End");
            }

            this.Start = Start;
            this.End = End;
        }


        public override string ToString()
        {
            return $"({Start}..{End})";
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public override bool Equals(object? Object)
        {
            return Object is Range<T> Range && Equals(Range);
        }


        public bool Equals(Range<T> Other)
        {
            return Start.CompareTo(Other.Start) == 0 && End.CompareTo(Other.End) == 0;
        }


        public static bool operator ==(Range<T> A, Range<T> B) => A.Equals(B);

        public static bool operator !=(Range<T> A, Range<T> B) => !A.Equals(B);


        public bool IsInside(T Value)
        {
            return Value.CompareTo(Start) >= 0 && Value.CompareTo(End) < 0;
        }

        public bool IsInside<R>(R Range) where R : IRange<T>
        {
            return IsInside(Range.Start) && Range.End.CompareTo(End) <= 0;
        }

        public bool Overlap<R>(R Range) where R : IRange<T>
        {
            return Range.End.CompareTo(Start) > 0 && Range.Start.CompareTo(End) < 0;
        }
    }
}