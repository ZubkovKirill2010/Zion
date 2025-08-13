namespace Zion
{
    public static class ZArray
    {
        public static T[] Concat<T>(params T[][] Arrays)
        {
            if (Arrays.IsNullOrEmpty())
            {
                return System.Array.Empty<T>();
            }

            int Index = 0;
            int Count = Arrays.Summarize(Array => Array.Length);
            T[] Result = new T[Count];

            foreach (T[] Array in Arrays)
            {
                foreach (T Item in Array)
                {
                    Result[Index++] = Item;
                }
            }

            return Result;
        }
    }
}