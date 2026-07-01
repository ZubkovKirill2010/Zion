using System.Diagnostics.CodeAnalysis;

namespace Zion.STP.Dynamic
{
    public abstract class TextPointer<T> : IEquatable<T>, IEqualityComparer<T>, IComparable<T> where T : TextPointer<T>
    {
        #region Properties
        public abstract bool IsValid { get; }

        #endregion

        #region Operators
        public static bool operator ==(TextPointer<T> A, TextPointer<T> B)
        {
            return A.Equals(B);
        }

        public static bool operator !=(TextPointer<T> A, TextPointer<T> B)
        {
            return !A.Equals(B);
        }

        #endregion

        #region AbstractMethods
        public abstract int CompareTo(T? Other);

        public abstract bool Equals(T? Other);

        public abstract T Sum(T Other);

        public abstract T Subtract(T Other);
        #endregion

        #region OverrideMethods
        public sealed override bool Equals(object? Object)
        {
            return Object is T Pointer && Equals(Pointer);
        }

        #endregion

        #region IEqualityComparer
        public bool Equals(T? A, T? B)
        {
            return EqualityComparer<T>.Default.Equals(A, B);
        }

        public int GetHashCode([DisallowNull] T Object)
        {
            return Object.GetHashCode();
        }

        #endregion
    }
}