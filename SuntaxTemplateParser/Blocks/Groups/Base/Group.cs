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


        public override IEnumerable<ColorChar> Enumerate(StringView String, int Start)
        {
            foreach (Block IBlock in this)
            {
                foreach (ColorChar Char in IBlock.Enumerate(String, Start))
                {
                    yield return Char;
                }
                Start += IBlock.Length;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}