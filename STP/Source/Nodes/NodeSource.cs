using System.Collections;

namespace Zion.STP
{
    public sealed class NodeSource<Node> : IEnumerable<Node> where Node : INode
    {
        private readonly List<Node> Source;
        public  readonly int Count;

        public NodeSource(List<Node> Source)
        {
            this.Source = Source.NotNull();
            this.Count  = Source.Count;
        }

        public Node this[int Index]
        {
            get => Source[Index];
        }
        public Node this[Index Index]
        {
            get => Source[Index];
        }


        public IEnumerator<Node> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}