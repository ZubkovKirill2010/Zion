using System.Diagnostics.CodeAnalysis;

namespace Zion.STP.Dynamic
{
    public abstract class TextPointer<T> : IEquatable<T>, IEqualityComparer<T>, IComparable<T> where T : TextPointer<T>
    {
        public static bool operator ==(TextPointer<T> A, TextPointer<T> B)
        {
            return A.Equals(B);
        }

        public static bool operator !=(TextPointer<T> A, TextPointer<T> B)
        {
            return !A.Equals(B);
        }


        public sealed override bool Equals(object? Object)
        {
            return Object is T Pointer && Equals(Pointer);
        }


        public bool Equals(T? A, T? B)
        {
            return EqualityComparer<T>.Default.Equals(A, B);
        }

        public int GetHashCode([DisallowNull] T Object)
        {
            return Object.GetHashCode();
        }


        public abstract bool Equals(T? Other);

        public abstract int CompareTo(T? Pointer);
    }
}