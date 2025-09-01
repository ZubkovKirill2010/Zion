namespace Zion
{
    public static class HashSet
    {
        public static HashSet<T> Empty<T>()
        {
            return new HashSet<T>(Array.Empty<T>());
        }
    }
}