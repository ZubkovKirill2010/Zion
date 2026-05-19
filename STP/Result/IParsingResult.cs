namespace Zion.STP
{
    public interface IParsingResult<T, Node> where Node : STP.Node
    {
        bool GetResult(NodeSource<Node> Source, out T Result);
    }
}