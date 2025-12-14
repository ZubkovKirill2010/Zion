using System.Collections;

namespace Zion.STP
{
    public abstract class Group : Block, IEnumerable<Block>
    {
        public abstract int Count { get; }

        public abstract Block this[int Index]
        {
            get;
        }

        public abstract IEnumerator<Block> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}