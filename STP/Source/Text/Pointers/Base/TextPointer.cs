namespace Zion.STP.Dynamic
{
    public abstract class TextPointer<T> : IComparable<T> where T : TextPointer<T>
    {
        public abstract int CompareTo(T? Pointer);
    }
}