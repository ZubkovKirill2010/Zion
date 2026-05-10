namespace Zion.STP
{
    public interface IParsingResult<T, Node> where Node : STP.Node
    {
        public bool GetResult(NodeSource<Node> Source, out T Result);
    }
}