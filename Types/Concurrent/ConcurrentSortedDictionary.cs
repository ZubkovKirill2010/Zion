namespace Zion
{
    public class ConcurrentSortedDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        private readonly ReaderWriterLockSlim Lock = new();
        private readonly SortedDictionary<TKey, TValue> Dictionary = new();

        public bool IsEmpty
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Dictionary.Count == 0;
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }


        public bool TryGetValue(TKey Key, out TValue Value)
        {
            Lock.EnterReadLock();
            try
            {
                return Dictionary.TryGetValue(Key, out Value!);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public TValue GetOrAdd(TKey Key, Func<TKey, TValue> ValueFactory)
        {
            Lock.EnterUpgradeableReadLock();
            try
            {
                if (Dictionary.TryGetValue(Key, out TValue? existingValue))
                    return existingValue;

                Lock.EnterWriteLock();
                try
                {
                    TValue? newValue = ValueFactory(Key);
                    Dictionary[Key] = newValue;
                    return newValue;
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
            finally
            {
                Lock.ExitUpgradeableReadLock();
            }
        }

        public bool TryRemove(TKey Key, out TValue Value)
        {
            Lock.EnterWriteLock();
            try
            {
                return Dictionary.Remove(Key, out Value!);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public TKey? GetLastKey()
        {
            Lock.EnterReadLock();
            try
            {
                return Dictionary.Count > 0 ? Dictionary.Keys.Last() : default;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public void Clear()
        {
            Lock.EnterWriteLock();
            try
            {
                Dictionary.Clear();
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        

        public IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable()
        {
            Lock.EnterReadLock();
            try
            {
                foreach (KeyValuePair<TKey, TValue> kvp in Dictionary)
                    yield return kvp;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Dictionary.Values.ToArray();
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }
    }
}