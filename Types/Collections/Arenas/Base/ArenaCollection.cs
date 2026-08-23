using System.Collections;

namespace Zion
{
    public abstract class ArenaCollection<T> : IDisposable, IEnumerable<T>
    {
        protected ArenaSpan<T> Data { get; private set; }

        public bool IsDisposed => Data.IsDisposed;


        public ArenaCollection(ArenaSpan<T> Data)
        {
            this.Data = Data.NotNull();
        }


        public override string ToString()
        {
            return StringFormatter.ToString(this);
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public abstract IEnumerator<T> GetEnumerator();


        public void Expand(int Capacity)
        {
            if (Capacity > Data.Count)
            {
                Data = Data.Expand(Capacity);
            }
        }


        public void Dispose()
        {
            Data.Dispose();
        }
    }
}