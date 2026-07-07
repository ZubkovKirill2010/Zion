namespace Zion
{
    public sealed class ObjectPool<T>
    {
        #region Data
        private readonly List<T> Pool;

        public required Func<T> New { get; init => field = value.NotNull(); }

        public Action<T>? Remove { get; init; }
        public Func<T, T> Getter { get; init => field = value ?? BaseGet; } = BaseGet;

        #endregion

        #region Properties
        public int Count => Pool.Count;

        #endregion

        #region Constructors
        public ObjectPool()
        {
            Pool = new List<T>(8);
        }
        
        public ObjectPool(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Pool = new List<T>(Capacity);
        }

        #endregion

        #region PublicMethods
        public void Add(T Item)
        {
            Pool.Add(Item);
        }

        public T Get()
        {
            if (Count != 0)
            {
                T Item = Getter(Pool[^1]);
                Pool.RemoveAt(Count - 1);
                return Item;
            }
            return New();
        }

        public void Clear()
        {
            if (Remove is not null)
            {
                foreach (T Item in Pool)
                {
                    Remove(Item);
                }
            }
            Pool.Clear();
        }


        public void ProcessBatch(int MaxConcurrency, IEnumerable<T> Items, Action<T> Action)
        {
            ArgumentNullException.ThrowIfNull(Items);
            ArgumentNullException.ThrowIfNull(Action);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxConcurrency);

            using SemaphoreSlim Semaphore = new(MaxConcurrency, MaxConcurrency);
            List<Task> Tasks = new();

            foreach (T Item in Items)
            {
                Semaphore.Wait();

                Tasks.Add
                (
                    Task.Run(() =>
                    {
                        try
                        {
                            Action(Item);
                        }
                        finally
                        {
                            Semaphore.Release();
                            Add(Item);
                        }
                    })
                );
            }

            Task.WaitAll(Tasks);
        }

        #endregion

        #region PrivateMethods
        private static T BaseGet(T Value)
        {
            return Value;
        }

        #endregion
    }
}