namespace Zion
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T>? Collection)
        {
            return Collection is null || Collection.Count == 0;
        }
    }
}