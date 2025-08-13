namespace Zion
{
    public static class ArrayExtensions
    {
        public static T[] Add<T>(this T[] Array, T Value)
        {
            if (Array is null)
            {
                throw new NullReferenceException("Array is null");
            }

            T[] NewArray = new T[Array.Length + 1];
            for (int i = 0; i < Array.Length; i++)
            {
                NewArray[i] = Array[i];
            }
            NewArray[Array.Length] = Value;
            return NewArray;
        }
        public static T[] Insert<T>(this T[] Array, int Index, T Value)
        {
            if (Array is null)
            {
                throw new ArgumentNullException(nameof(Array));
            }
            if (Index < 0 || Index > Array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Index), "Index must be within array bounds");
            }

            T[] NewArray = new T[Array.Length + 1];

            System.Array.Copy(Array, 0, NewArray, 0, Index);

            NewArray[Index] = Value;

            System.Array.Copy(Array, Index, NewArray, Index + 1, Array.Length - Index);

            return NewArray;
        }

        public static T[] RemoveAt<T>(this T[] Array, int Index)
        {
            if (Array is null)
            {
                throw new NullReferenceException("Array is null");
            }
            if (Index < 0 || Index >= Array.Length)
            {
                throw new ArgumentOutOfRangeException($"Index out of array, Index = {Index}, Array.Length = {Array.Length}");
            }

            T[] NewArray = new T[Array.Length - 1];

            System.Array.Copy(Array, NewArray, Index);
            if (Index < Array.Length - 1)
            {
                System.Array.Copy(Array, Index + 1, NewArray, Index, Array.Length - Index - 1);
            }

            return NewArray;
        }

        public static T[] Clone<T>(this T[] Array) where T : struct
        {
            T[] Result = new T[Array.Length];
            for (int i = 0; i < Array.Length; i++)
            {
                Result[i] = Array[i];
            }
            return Result;
        }
    }
}