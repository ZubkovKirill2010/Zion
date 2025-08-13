namespace Zion
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this Dictionary<TKey, TValue> Dictionary)
        {
            Dictionary<TValue, TKey> Result = new Dictionary<TValue, TKey>(Dictionary.Count);

            foreach (KeyValuePair<TKey, TValue> Item in Dictionary)
            {
                Result.Add(Item.Value, Item.Key);
            }
            return Result;
        }
    }
}