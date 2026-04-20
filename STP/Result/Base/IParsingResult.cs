namespace Zion.STP
{
    public interface IParsingResult<T, Node> where Node : INode
    {
        public bool GetResult(NodeSource<Node> Source, out T Result);
    }
}