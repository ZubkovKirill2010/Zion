using System.Collections;

namespace Zion
{
    public abstract class ArenaCollection<T> : IDisposable, IEnumerable<T>
    {
        #region Properties
        protected ArenaSpan<T> Data { get; private set; }

        public bool IsDisposed => Data.IsDisposed;

        #endregion

        #region Constructors
        public ArenaCollection(ArenaSpan<T> Data)
        {
            this.Data = Data.NotNull();
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return StringFormatter.ToString(this);
        }

        #endregion

        #region PublicMethods
        public void Expand(int Capacity)
        {
            if (Capacity > Data.Count)
            {
                Data = Data.Expand(Capacity);
            }
        }

        public bool IsFrom(Arena<T> Arena)
        {
            return Data.IsFrom(Arena);
        }

        #endregion

        #region AbstractMethods
        protected abstract IEnumerator<int> GetIndexEnumerator();

        #endregion

        #region IDisposable
        public void Dispose()
        {
            Data.Dispose();
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator(GetIndexEnumerator());
        }

        #endregion
    }
}