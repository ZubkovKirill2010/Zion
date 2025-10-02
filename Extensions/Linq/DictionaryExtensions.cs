using System.Runtime;

namespace Zion
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Creates a new dictionary with keys and values swapped.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the original dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the original dictionary.</typeparam>
        /// <param name="Dictionary">The source dictionary to reverse.</param>
        /// <returns>A new dictionary where values become keys and keys become values.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Dictionary"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if duplicate values exist in the original dictionary.</exception>
        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this Dictionary<TKey, TValue> Dictionary)
        {
            Dictionary<TValue, TKey> Result = new Dictionary<TValue, TKey>(Dictionary.Count);

            foreach (KeyValuePair<TKey, TValue> Item in Dictionary)
            {
                Result.Add(Item.Value, Item.Key);
            }
            return Result;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> Dictionary, TKey Key, TValue Default)
        {
            return Dictionary.TryGetValue(Key, out TValue? Value) ? Value : Default;
        }

        public static TOut GetValue<TKey, TValue, TOut>(this IDictionary<TKey, TValue> Dictionary, TKey Key, TOut Default, Func<TValue, TOut> Converter)
        {
            return Dictionary.TryGetValue(Key, out TValue? Value) ? Converter(Value) : Default;
        }
    }
}