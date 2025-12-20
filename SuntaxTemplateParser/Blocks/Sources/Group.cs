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

        public override IEnumerable<ColorChar> Enumerate(StringView String, int Start)
        {
            foreach (Block Block in this)
            {
                foreach (ColorChar Char in Block.Enumerate(String, Start))
                {
                    yield return Char;
                }
                Start += Block.Length;
            }
        }
    }
}