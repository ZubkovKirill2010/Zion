using System.Collections;

namespace Zion.STP
{
    public struct EmptyGroupTemplate : IGroupTemplate
    {
        public ITemplate this[int Index] => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public IEnumerator<ITemplate> GetEnumerator()
        {
            yield break;
        }

        public bool IsMatch(StringView String, int Start, out Block Block)
        {
            Block = new EmptyBlock(String, this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}