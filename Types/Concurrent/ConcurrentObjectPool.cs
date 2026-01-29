using System.Collections.Concurrent;

namespace Zion
{
    public class ConcurrentObjectPool<T> where T : class
    {
        private readonly Func<T> Factory;
        private readonly ConcurrentBag<T> Pool = new();

        public ConcurrentObjectPool(Func<T> factory)
        {
            Factory = factory;
        }

        public T Get()
        {
            return Pool.TryTake(out T? Item) ? Item : Factory();
        }

        public void Return(T Item)
        {
            Pool.Add(Item);
        }
    }
}