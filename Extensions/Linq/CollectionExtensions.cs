namespace Zion
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Determines whether the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Collection">The collection to check.</param>
        /// <returns>true if the collection is null or empty; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T>? Collection)
        {
            return Collection is null || Collection.Count == 0;
        }
    }
}