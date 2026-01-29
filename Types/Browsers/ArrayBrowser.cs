namespace Zion
{
    public sealed class ArrayBrowser<T>
    {
        private readonly T[] Array;

        public readonly int Length;

        public ArrayBrowser(T[] Array)
        {
            ArgumentNullException.ThrowIfNull(Array, "Array");

            this.Array = Array;
            Length = Array.Length;
        }

        public T   this[int   Index] => Array[Index];
        public T   this[Index Index] => Array[Index];
        public T[] this[Range Range] => Array[Range];
    }
}