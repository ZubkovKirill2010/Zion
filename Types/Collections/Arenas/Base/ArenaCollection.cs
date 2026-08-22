using System.Collections;

namespace Zion
{
    public abstract class ArenaCollection<T> : IDisposable, IEnumerable<T>
    {
        protected readonly ArenaSpan<T> Data;


        public ArenaCollection(ArenaSpan<T> Data)
        {
            this.Data = Data.NotNull();
        }


        public override string ToString()
        {
            return StringFormatter.ToString(this);
        }


        public void Dispose()
        {
            Data.Dispose();
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public abstract IEnumerator<T> GetEnumerator();
    }
}